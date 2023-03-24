using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public enum DamageType {
		Ranged,
		Contact,
		Slash
	}

	public struct DamageModel {
		public DamageType damageType;          //�˺�����
		public int amount;                     //������
		public float knockback;                //������
		public EntityBase dealer;              //��ɹ�����
		public ProjectileBase dealerProjectile;//��ɹ������䵯
		public int direction;                  //��������, ֵ�ĺ���ο�Direction��
	}

	public class EntityBase:MonoBehaviour {

		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		protected SpriteRenderer spriteRenderer;
		protected Animator animator;

		protected CombatRoomController room;

		#region ���� ��prefab�б༭ ��Ҫ��̬�޸�
		[field: SerializeField] public float acceleration { get; protected set; }           //��������   
		[field: SerializeField] public float maxSpeed { get; protected set; }               //�������   
		[field: SerializeField] public int attackBasePower { get; protected set; }          //���������� 
		[field: SerializeField] public float attackCd { get; protected set; }               //�������   
		[field: SerializeField] public float knockbackPower { get; protected set; }         //������
		[field: SerializeField] public int maxHp { get; protected set; }                    //�������ֵ
		[field: SerializeField] public GameObject[] projectiles { get; protected set; }     //�䵯���� �����������ѡȡ
		[field: SerializeField] public float attackRangeMin { get; protected set; }         //��С��������
		[field: SerializeField] public float attackRangeMax { get; protected set; }         //��󹥻�����
		[field: SerializeField] public float projectileVelocityY { get; protected set; }    //Ͷ������������� Խ�����������Խ��Խ�������� ��Ͷ��������Ϊ0��ΪͶ�����ˮƽ����
		[field: SerializeField] public float maxPredictSpeed { get; protected set; }        //���Ԥ������ ��Ԥ�й���ʱ��Ŀ�������������ƶ�����������������ƶ�
		[field: SerializeField] public GameObject[] damageVfx { get; protected set; }       //�˺���Ч �����������ѡȡ

		[SerializeField] protected AudioClip soundAttack;
		[SerializeField] protected AudioClip soundHit;
		[SerializeField] protected AudioClip soundDeath;

		//��ȡ��ǰ��ɫ���������Ĳ���
		protected virtual DamageModel GetDamage() {
			DamageModel result = new DamageModel();
			result.amount=Mathf.RoundToInt(attackBasePower*powerBuff);
			result.knockback=knockbackPower*knockbackBuff;
			result.dealer=this;
			return result;
		}

		//��ǰ״̬ ������״̬ÿ�̿�ʼ�ᱻ��ֵ ����ӦUpdateStats�Ĵ����и�����Щֵ
		public int hp { get; protected set; }          //��ǰ����ֵ
		[HideInInspector] public float powerBuff;      //��ǿ������
		[HideInInspector] public float knockbackBuff;  //��ǿ����
		[HideInInspector] public float cdSpeed;        //���ӹ�����ȴ�ٶ�
		[HideInInspector] public float speedBuff;      //�����ƶ��ٶȺͼ��ٶ�

		#endregion

		//��ɫ�ĳ��� �ο�Direction��
		protected int direction = Direction.right;

		//����״̬���¼� �ı��ɫ״̬�Ĵ������ڹ�ע����¼��ĺ����е���
		public static event EventHandler UpdateStats;
		void OnUpdateStats() {
			powerBuff=1;
			knockbackBuff=1;
			cdSpeed=1;
			speedBuff=1;
			UpdateStats?.Invoke(this);

		}

		public DamageModel lastDamage { get; protected set; }            //���һ���ܵ����˺�
		public static event System.EventHandler<DamageModel> DamageEvent;//�ܵ��˺�ʱ�������¼�


		//�Խ�ɫ����˺�
		public virtual void Damage(DamageModel e) { //�ܵ��˺�
			if(e.amount>0) {
				AudioController.PlayAudio(soundHit,transform.position);
				animator.SetTrigger("hit");
			}
			hp-=e.amount;
			DoKnockback(e.knockback,e.direction);

			int vfxIndex = Random.Range(0,damageVfx.Length);
			VfxPool.Create(damageVfx[vfxIndex],transform.position,e.direction);

			DamageEvent?.Invoke(this,e);
			lastDamage=e;

			if(hp<=0) OnDeath();

		}

		//�ָ�Ѫ��
		public virtual void Heal(int amount) {
			hp+=amount;
			if(hp>maxHp) hp=maxHp;
		}

		//������ʱ�������¼� ��Ҫ��������ɫ����ʱִ�д������ע����¼�
		public static event EventHandler Death;
		//����ʱ�����Ĵ���
		protected virtual void OnDeath() {
			AudioController.PlayAudio(soundDeath,transform.position);
			Death?.Invoke(this);
		}

		protected virtual void Start() {
			hp=maxHp;
			StartMove();
			positionInList=entities.AddLast(this);

			collider=GetComponent<Collider2D>();
			spriteRenderer=GetComponentInChildren<SpriteRenderer>();
			animator=GetComponent<Animator>();

			room=GetComponentInParent<CombatRoomController>();


			buffSlot=new BuffSlot();
			buffSlot.Init(this);

			for(int i = 0;i<attacks.Length;i++) attacks[i].id=i;
		}

		protected virtual void OnDestroy() {
			if(positionInList!=null&&entities!=null) entities.Remove(positionInList);
		}

		protected virtual void Update() {

			if(room!=CombatRoomController.currentRoom) return;

			UpdateTarget();
			UpdateMove();

			transform.localScale=(direction==Direction.left) ? new Vector3(-1,1,1) : new Vector3(1,1,1);
			animator.SetFloat("speed",Mathf.Abs(velocity.x));
			if(currensState!=StateKnockback) animator.SetBool("inKnockback",false);

			buffSlot.Update();
		}

		float distanceMoved;
		protected virtual void FixedUpdate() {

			if(room!=CombatRoomController.currentRoom) return;

			OnUpdateStats();
			currensState();

			UpdateLimitation();

			if(currensState==StateMove) {
				distanceMoved+=Time.deltaTime*Mathf.Abs(velocity.x);
				if(distanceMoved>2.5f) {
					distanceMoved-=2.5f;
					AudioClip soundToPlay = CombatController.instance.walkSounds[Random.Range(0,2)];
					AudioController.PlayAudio(soundToPlay,transform.position);
				}
			}

		}

		protected void UpdateLimitation() {

			float startX = room.startX;
			float endX = room.endX;

			Vector3 position = transform.position;

			if(transform.position.x<startX) {
				if(velocity.x<0) velocity.x=0;
				if(position.x<startX) position.x=startX;
				transform.position+=Vector3.right*(startX-transform.position.x);
			}
			if(transform.position.x>endX) {
				if(velocity.x>0) velocity.x=0;
				if(position.x>endX) position.x=endX;
				transform.position+=Vector3.right*(endX-transform.position.x);
			}

			transform.position=position;

		}

		//�ƶ����
		protected Vector2 velocity;
		virtual protected void UpdateMove() {
			Vector2 position = transform.position;
			position+=velocity*Time.deltaTime;
			if(position.y<room.transform.position.y) position.y=room.transform.position.y;
			transform.position=position;
		}

		public Void currensState { get; protected set; }

		/// <summary>
		/// ׷�����
		/// </summary>
		protected virtual void StartMove() {
			currensState=StateMove;
			velocity=Vector2.zero;
		}
		protected virtual void StateMove() {

		}

		protected virtual void DoKnockback(float knockback,int direction) {
			/*
				transform.position+=(Vector3)Direction.GetVector(direction)*knockback*0.1f;
					
				timeSinceKnockback=0;
				currentKnockback=knockback;
				knockbackDirection=direction;
				currensState=StateKnockback;
				animator.SetBool("inKnockback",true);
			*/

			buffSlot[typeof(BuffKnockback)].stacks+=(direction==Direction.right ? 1 : -1)*knockback;

		}

		const float knockbackTime = 0.1f;
		const float knockbackHeight = 0.5f;

		float currentKnockback;
		float timeSinceKnockback;
		int knockbackDirection;
		public bool isKnockbacked { get { return buffSlot.ContainsBuff(typeof(BuffKnockback)); } }
		protected virtual void StateKnockback() {
			timeSinceKnockback+=Time.deltaTime;
			Vector2 position = transform.position;

			float curveX = (timeSinceKnockback-0.5f*knockbackTime)/knockbackTime;
			position.y=room.transform.position.y+knockbackHeight*(-curveX*curveX+0.25f);
			position+=Direction.GetVector(knockbackDirection)*Time.deltaTime*currentKnockback/knockbackTime;


			curveX=(timeSinceKnockback+Time.deltaTime-0.5f*knockbackTime)/knockbackTime;
			float nextY = room.transform.position.y+knockbackHeight*(-curveX*curveX+0.25f);

			velocity=Direction.GetVector(knockbackDirection)*currentKnockback/knockbackTime;
			velocity.y=(nextY-position.y)/Time.deltaTime;

			transform.position=position;
			direction=Direction.Reverse(knockbackDirection);
			if(timeSinceKnockback>=knockbackTime) StartMove();
		}

		#region ����Ŀ��

		protected virtual void UpdateTarget() { target=GetNearestTarget(); }

		protected EntityBase target;

		protected virtual float targetX => target==null ? transform.position.x : target.transform.position.x;

		protected virtual float distanceToTarget => Mathf.Abs(targetX-transform.position.x);

		#endregion

		#region ��������/����״̬

		[SerializeField] protected float attackChance = 1;        //�ƶ���������빥��״̬�ĸ���
		[SerializeField] protected AttackStateData[] attacks;     //�������й���״̬���б�
		public float timeAfterAttack { get; protected set; }
		//�ý�ɫ�����ж��Ƿ񹥻�
		protected virtual void UpdateAttack() {

			timeAfterAttack+=Time.deltaTime*cdSpeed;

			EntityBase target = GetNearestTarget();

			if(target) {
				direction=(target.transform.position.x<transform.position.x) ? Direction.left : Direction.right;
				if(timeAfterAttack>attackCd) {

					Attack(target);
				}
			}

		}

		//���Ź�������

		public float ���������ƶ��ٶ�;
		protected float overrideSpeed {
			get => ���������ƶ��ٶ�;
			set => ���������ƶ��ٶ�=value;
		}

		protected int attackIndex;

		protected readonly static SortedSet<AttackStateData> attackStatesBuffer = new SortedSet<AttackStateData>();
		protected readonly static SortedSet<AttackStateTransistion> transitionBuffer = new SortedSet<AttackStateTransistion>();

		protected virtual void StartRandomAttack() {
			float weightTotal = 0;
			float distanceToTarget = this.distanceToTarget;

			//�ҳ����ܵ�ת��Ŀ��
			attackStatesBuffer.Clear();
			foreach(var i in attacks) {
				if(i.maxDistance<distanceToTarget||i.minDistance>distanceToTarget) continue;
				weightTotal+=i.startWeight;
				attackStatesBuffer.Add(i);
			}

			//ѡ��ʵ�ʵ�ת��Ŀ��(��������)
			float randomFactor = Random.Range(0,weightTotal);
			AttackStateData targetState = null;
			foreach(var i in attackStatesBuffer) {
				randomFactor-=i.startWeight;
				if(randomFactor<=Mathf.Epsilon) {
					targetState=i;
					break;
				}
			}

			//�ж�Ҫת�Ƶ�����������������
			if(Utility.Chance(1-attackChance)||targetState==null) {
				//��������
				StartMove();
			} else {
				//���й���
				StartAttack(targetState.id);
			}

		}
		protected virtual void StartAttack(int attackIndex) {
			direction=targetX>transform.position.x ? Direction.right : Direction.left;
			this.attackIndex=attackIndex;
			animator.SetTrigger($"attack{attackIndex}");
			currensState=StateAttack;
		}
		protected virtual void StateAttack() {

			velocity=overrideSpeed*Direction.GetVector(direction);
			overrideSpeed=0;

		}

		//�ڴ�������ǰ���������Ľӿ�
		protected virtual void EndAttack() {
			animator.SetTrigger("attackEnd");
			StartMove();
		}

		//ͨ�������ڹ�������ʱ����
		public virtual void �����¼�_AttackEnd() {
			transitionBuffer.Clear();
			float distanceToTarget = this.distanceToTarget;
			AttackStateData currentAttack = attacks[attackIndex];
			float weightTotal = 0;

			//ͳ�ƿ��õ�Ŀ��״̬
			foreach(var i in currentAttack.transitionList) {
				switch(i.type) {
				case AttackStateTransistionType.Attack:
					if(distanceToTarget<attacks[i.attackId].minDistance||distanceToTarget>attacks[i.attackId].maxDistance) break;
					weightTotal+=i.weight;
					transitionBuffer.Add(i);
					break;

				case AttackStateTransistionType.Move:
					weightTotal+=i.weight;
					transitionBuffer.Add(i);
					break;

				}


			}

			//�ж�����Ŀ��
			float randomFactor = Random.Range(0,weightTotal);
			AttackStateTransistion transistion = null;
			foreach(var i in transitionBuffer) {
				randomFactor-=i.weight;
				if(randomFactor<=Mathf.Epsilon) {
					transistion=i;
					break;
				}
			}

			if(transistion==null||transistion.type==AttackStateTransistionType.Move) StartMove();
			else StartAttack(transistion.attackId);

		}
		#endregion

		#region Ĭ�Ϲ����¼�


		//��ȡӦ�ù����ĵ���
		protected virtual EntityBase GetNearestTarget() {
			//�ҵ�����
			EntityBase target = null;
			float targetDistance = float.MaxValue;

			foreach(var i in entities) {

				if(!i.gameObject.activeInHierarchy) continue;

				if((i is EntityFriendly)==(this is EntityFriendly)) continue;
				float x = i.transform.position.x;
				float dist = Mathf.Abs(x-transform.position.x);
				if(dist<attackRangeMin||dist>attackRangeMax) continue;

				if(dist<targetDistance) {
					targetDistance=dist;
					target=i;
				}

			}
			return target;
		}

		//��Ҫ���� ��ִ����������ж���ι���
		protected virtual ProjectileBase Attack(EntityBase target) {

			AudioController.PlayAudio(soundAttack,transform.position);

			int projectileType = Random.Range(0,projectiles.Length);

			animator.SetTrigger("attack");
			timeAfterAttack=0;
			return ProjectilePool.Create(
				projectiles[projectileType],
				transform.position,
				ProjectileVelocity(target.transform.position,target.velocity,projectileType),
				target,
				this is EntityFriendly,
				GetDamage()
			);

		}

		//�ж��ӵ��ķ����ٶ� ��׼+Ԥ��
		protected bool projectileParametersGet;
		protected float[] projectileGravity;
		protected float[] travelTime;
		//�����䵯�ķ����ٶ�
		protected virtual Vector2 ProjectileVelocity(Vector2 target,Vector2 velocity,int projectileType) {
			if(!projectileParametersGet) {
				projectileParametersGet=true;

				int cnt = projectiles.Length;
				projectileGravity=new float[cnt];
				travelTime=new float[cnt];

				for(int i = 0;i<cnt;i++) {
					projectileGravity[i]=projectiles[i].GetComponent<ProjectileBase>().gravity;
					travelTime[i]=2*projectileVelocityY/projectileGravity[i];
					if(projectileGravity[i]==0) travelTime[i]=0;
				}

			}

			float arriveX = target.x+Mathf.Clamp(velocity.x,-maxPredictSpeed,maxPredictSpeed)*travelTime[projectileType];

			float velocityX = (arriveX-transform.position.x)/travelTime[projectileType];
			if(travelTime[projectileType]==0) velocityX=0;
			return new Vector2(velocityX,projectileVelocityY);


		}

		#endregion

		protected BuffSlot buffSlot;


	}


}