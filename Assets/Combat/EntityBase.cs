using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public struct DamageModel {
		public int amount;          //������
		public float knockback;     //������
		public EntityBase dealer;   //��ɹ�����
		public int direction;       //��������, ֵ�ĺ���ο�Direction��
	}

	public class EntityBase:MonoBehaviour {


		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		protected SpriteRenderer spriteRenderer;
		protected Animator animator;

		//���� ��prefab�б༭ ��Ҫ��̬�޸�
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

		//�Խ�ɫ����˺�
		public virtual void Damage(DamageModel e) {

			hp-=e.amount;
			StartKnockback(e.knockback,e.direction);

			int vfxIndex = Random.Range(0,damageVfx.Length);
			VfxPool.Create(damageVfx[vfxIndex],transform.position,e.direction);

			if(hp<=0) OnDeath();

		}

		//������ʱ�������¼� ��Ҫ��������ɫ����ʱִ�д������ע����¼�
		public static event EventHandler Death;
		//����ʱ�����Ĵ���
		protected virtual void OnDeath() {
			Death?.Invoke(this);
		}

		protected virtual void Start() {
			hp=maxHp;
			previousPosition=transform.position;
			StartMove();
			positionInList=entities.AddLast(this);

			collider=GetComponent<Collider2D>();
			spriteRenderer=GetComponentInChildren<SpriteRenderer>();
			animator=GetComponent<Animator>();
		}

		protected virtual void OnDestroy() {
			entities.Remove(positionInList);
		}

		private void Update() {
			UpdateMove();
			transform.localScale=(direction==Direction.left) ? new Vector3(-1,1,1) : new Vector3(1,1,1);
			animator.SetFloat("speed",Mathf.Abs(velocity.x));
			if(currensState!=StateKnockback) animator.SetBool("inKnockback",false);
		}

		protected virtual void FixedUpdate() {
			OnUpdateStats();
			currensState();
		}

		//�ƶ����
		protected Vector2 velocity;
		protected Vector2 previousPosition;
		virtual protected void UpdateMove() {
			Vector2 position = transform.position;
			position+=velocity*Time.deltaTime;
			if(position.y<0) position.y=0;
			transform.position=position;
		}

		protected Void currensState;

		protected virtual void StartMove() {
			currensState=StateMove;
			velocity=Vector2.zero;
		}
		protected virtual void StateMove() {

		}

		protected virtual void StartKnockback(float knockback,int direction) {
			timeSinceKnockback=0;
			currentKnockback=knockback;
			knockbackDirection=direction;
			currensState=StateKnockback;
			animator.SetBool("inKnockback",true);
		}

		const float knockbackTime = 0.1f;
		const float knockbackHeight = 0.5f;

		float currentKnockback;
		float timeSinceKnockback;
		int knockbackDirection;
		protected virtual void StateKnockback() {
			timeSinceKnockback+=Time.deltaTime;
			Vector2 position = previousPosition;

			float curveX = (timeSinceKnockback-0.5f*knockbackTime)/knockbackTime;
			position.y=knockbackHeight*(-curveX*curveX+0.25f);
			position+=Direction.GetVector(knockbackDirection)*Time.deltaTime*currentKnockback/knockbackTime;


			curveX=(timeSinceKnockback+Time.deltaTime-0.5f*knockbackTime)/knockbackTime;
			float nextY = knockbackHeight*(-curveX*curveX+0.25f);

			velocity=Direction.GetVector(knockbackDirection)*currentKnockback/knockbackTime;
			velocity.y=(nextY-position.y)/Time.deltaTime;

			transform.position=position;
			previousPosition=position;
			direction=Direction.Reverse(knockbackDirection);
			if(timeSinceKnockback>=knockbackTime) StartMove();
		}

		//����
		public float timeAfterAttack { get; protected set; }
		//�ý�ɫ�����ж��Ƿ񹥻�
		protected virtual void UpdateAttack() {

			timeAfterAttack+=Time.deltaTime;

			//�ҵ�����
			EntityBase target = null;
			float targetDistance = float.MaxValue;

			foreach(var i in entities) {

				if((i is EntityFriendly)==(this is EntityFriendly)) continue;
				float x = i.transform.position.x;
				float dist = Mathf.Abs(x-transform.position.x);
				if(dist<attackRangeMin||dist>attackRangeMax) continue;

				if(dist<targetDistance) {
					targetDistance=dist;
					target=i;
				}

				if(target) {
					direction=(target.transform.position.x<transform.position.x) ? Direction.left : Direction.right;
					if(timeAfterAttack>attackCd) {
						Attack(target);
					}
				} else {
					direction=(this is EntityFriendly) ? Direction.right : Direction.left;
				}

			}

		}

		//��Ҫ���� ��ִ����������ж���ι���
		protected virtual ProjectileBase Attack(EntityBase target) {

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

			if(travelTime[projectileType]==0) return (target.x>transform.position.x ? Vector2.right : Vector2.left)*projectileVelocityY;
			float velocityX = (arriveX-transform.position.x)/travelTime[projectileType];
			return new Vector2(velocityX,projectileVelocityY);


		}

	}



}