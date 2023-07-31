using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public enum DamageType {
		Ranged,
		Contact,
		Slash,
		HpLoss
	}

	public struct DamageModel {
		public DamageType damageType;          //伤害类型
		public int amount;                     //攻击力
		public float knockback;                //击退力
		public EntityBase dealer;              //造成攻击者
		public ProjectileBase dealerProjectile;//造成攻击的射弹
		public int direction;                  //攻击方向, 值的含义参考Direction类
	}

	[System.Serializable]
	public class AttackData {
		[Tooltip("射弹种类")]
		[field: SerializeField] public GameObject projectile { get; private set; }

		[Tooltip("投射物的纵向速率 越大则横向速率越慢越难以命中 若投射物重力为0则为投射物的水平速率")]
		[field: SerializeField] public float projectileVelocityY { get; private set; }

		[Tooltip("击中时获得的能量值")]
		[field: SerializeField] public float manaGain { get; private set; }

		[Tooltip("攻击声音")]
		[field: SerializeField]
		public AudioClip sound { get; private set; }

		public float projectileGravity { get; private set; }
		public float travelTime { get; private set; }
		public void InitParams() {
			projectileGravity=projectile.GetComponent<ProjectileBase>().gravity;
			travelTime=2*projectileVelocityY/projectileGravity;
			if(projectileGravity==0) travelTime=0;
		}
	}

	public class EntityBase:MonoBehaviour {

		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		[field: SerializeField] public SpriteRenderer spriteRenderer { get; protected set; }
		protected Animator animator;
		protected EntityAdditionalFunctionBase additional;

		protected CombatRoomController room;

		#region 属性 在prefab中编辑 不要动态修改
		[Tooltip("加速能力")]
		[field: SerializeField] public float acceleration { get; protected set; }
		[Tooltip("最大速率")]
		[field: SerializeField] public float maxSpeed { get; protected set; }
		[Tooltip("基础攻击力")]
		[field: SerializeField] public int attackBasePower { get; protected set; }
		[Tooltip("击退力")]
		[field: SerializeField] public float knockbackPower { get; protected set; }
		[Tooltip("最大生命值")]
		[field: SerializeField] public int maxHp { get; protected set; }
		[Tooltip("远程攻击/平A技能种类 在数组中随机选取")]
		[field: SerializeField] public AttackData[] attacks { get; protected set; }
		[Tooltip("最大预判速率 在预判攻击时若目标大于这个速率移动则认为是做以这个速率移动")]
		[field: SerializeField] public float maxPredictSpeed { get; protected set; }
		[Tooltip("伤害特效 在数组中随机选取")]
		[field: SerializeField] public GameObject[] damageVfx { get; protected set; }
		[Tooltip("阵营")]
		[field: SerializeField] public bool isFriendly { get; private set; }

		[Tooltip("受伤时使用的shader")]
		[SerializeField] Material materialHit;
		Material materialDefault;

		Transform shootPosition;

		[SerializeField] protected AudioClip soundHit;
		[SerializeField] protected AudioClip soundDeath;

		//获取当前角色正常攻击的参数
		public virtual DamageModel GetDamage() {
			DamageModel result = new DamageModel();
			result.amount=Mathf.RoundToInt(attackBasePower*powerBuff);
			result.knockback=knockbackPower*knockbackBuff;
			result.dealer=this;
			return result;
		}

		//当前状态 后三个状态每刻开始会被充值 在相应UpdateStats的代码中更改这些值
		public int hp { get; protected set; }          //当前生命值
		[HideInInspector] public float powerBuff;      //增强攻击力
		[HideInInspector] public float knockbackBuff;  //增强击退
		[HideInInspector] public float cdSpeed;        //增加攻击冷却速度
		[HideInInspector] public float speedBuff;      //增加移动速度和加速度

		#endregion

		//角色的朝向 参考Direction类
		protected int direction = Direction.right;

		//更新状态的事件 改变角色状态的代码请在关注这个事件的函数中调用
		public static event EventHandler UpdateStats;
		void OnUpdateStats() {
			powerBuff=1;
			knockbackBuff=1;
			cdSpeed=1;
			speedBuff=1;
			buffSlot.Update();
			UpdateStats?.Invoke(this);

		}


		public DamageModel lastDamage { get; protected set; }            //最后一次受到的伤害
		public static event System.EventHandler<DamageModel> DamageEvent;//受到伤害时触发的事件


		//对角色造成伤害
		float timeAfterHit;
		public virtual void Damage(DamageModel e) { //受到伤害

			if(e.amount>0&&e.damageType!=DamageType.HpLoss) {
				AudioController.PlayAudio(soundHit,transform.position);
				animator.SetTrigger("hit");
				timeAfterHit=0;
				if(damageVfx.Length!=0) {
					int vfxIndex = Random.Range(0,damageVfx.Length);
					VfxPool.Create(damageVfx[vfxIndex],transform.position,e.direction);
				}
			}
			hp-=e.amount;

			if(e.damageType!=DamageType.HpLoss) {
				DoKnockback(e.knockback,e.direction);
				DamageEvent?.Invoke(this,e);
			}
			lastDamage=e;
			if(hp<=0) {
				OnDeath();
			}
		}

		//恢复血量 
		public virtual void Heal(int amount) {
			hp+=amount;
			if(hp>maxHp) hp=maxHp;
		}

		//在死亡时触发的事件 想要在其他角色死亡时执行代码请关注这个事件
		public static event EventHandler Death;
		//死亡时触发的代码
		protected virtual void OnDeath() {
			AudioController.PlayAudio(soundDeath,transform.position);
			Death?.Invoke(this);
		}

		protected virtual void Start() {

			previousPosition=transform.position;
			nextPosition=transform.position;

			hp=maxHp;
			StartMove();
			positionInList=entities.AddLast(this);

			collider=GetComponent<Collider2D>();
			if(spriteRenderer==null) spriteRenderer=GetComponentInChildren<SpriteRenderer>();
			animator=GetComponent<Animator>();
			additional=GetComponent<EntityAdditionalFunctionBase>();
			if(additional==null)
				additional=gameObject.AddComponent(typeof(EntityAdditionalFunctionBase)) as EntityAdditionalFunctionBase;

			room=GetComponentInParent<CombatRoomController>();

			buffSlot=new BuffSlot();
			buffSlot.Init(this);
			shootPosition=transform.Find("shootPosition");

			materialDefault=spriteRenderer.material;
		}

		protected virtual void OnDestroy() {
			if(positionInList!=null&&entities!=null) entities.Remove(positionInList);
		}

		protected virtual void Update() {

			if(room!=CombatRoomController.currentRoom) return;
			timeAfterHit+=Time.deltaTime;

			UpdateTarget();
			UpdateMove();

			transform.localScale=(direction==Direction.left) ? new Vector3(-1,1,1) : new Vector3(1,1,1);
			animator.SetFloat("speed",Mathf.Abs(velocity.x));
			animator.SetFloat("forwardSpeed",velocity.x*Direction.GetX(direction));
			if(currensState!=StateKnockback) animator.SetBool("inKnockback",false);

			UpdateMaterial();
		}

		public void MovePosition(Vector3 target) {
			Vector3 offset = target-transform.position;
			transform.position+=offset;
			previousPosition+=offset;
			nextPosition+=offset;
		}
		float distanceMoved;
		Vector3 previousPosition;
		Vector3 nextPosition;
		Vector3 cumulatedMovement;
		void FixedUpdateMove() {
			transform.position=nextPosition;
			previousPosition=transform.position;
			cumulatedMovement=Vector3.zero;
		}

		protected virtual void FixedUpdate() {
			FixedUpdateMove();
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

		protected virtual void UpdateMaterial() {
			if(timeAfterHit>0.01f&&timeAfterHit<0.1f) spriteRenderer.material=materialHit;
			else spriteRenderer.material=materialDefault;
		}

		//移动相关
		protected Vector2 velocity;
		virtual protected void UpdateMove() {
			cumulatedMovement+=(Vector3)velocity*2*Time.deltaTime;
			nextPosition=previousPosition+(Vector3)velocity*2*Time.fixedDeltaTime;
			Vector3 position = previousPosition+cumulatedMovement;
			if(position.y<room.transform.position.y) position.y=room.transform.position.y;
			transform.position=position;
		}

		public Void currensState { get; protected set; }

		/// <summary>
		/// 追踪玩家
		/// </summary>
		protected virtual void StartMove() {
			currensState=StateMove;
			velocity=Vector2.zero;
		}
		protected virtual void StateMove() {

		}

		const float knockbackUnit = 0.1f;
		public virtual void DoKnockback(float knockback,int direction) {
			/*
				transform.position+=(Vector3)Direction.GetVector(direction)*knockback*0.1f;
					
				timeSinceKnockback=0;
				currentKnockback=knockback;
				knockbackDirection=direction;
				currensState=StateKnockback;
				animator.SetBool("inKnockback",true);
			*/

			buffSlot[typeof(BuffKnockback)].stacks+=(direction==Direction.right ? 1 : -1)*knockback*knockbackUnit;

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

		#region 攻击目标

		protected virtual void UpdateTarget() { target=GetNearestTarget(); }

		protected EntityBase target;

		protected virtual float targetX => target==null ? transform.position.x : target.transform.position.x;

		protected virtual float distanceToTarget => Mathf.Abs(targetX-transform.position.x);

		#endregion

		#region 攻击

		public float timeAfterAttack { get; protected set; }

		#endregion

		#region 默认攻击事件

		//获取应该攻击的敌人
		protected virtual EntityBase GetNearestTarget() {
			//找到敌人
			EntityBase target = null;
			float targetDistance = float.MaxValue;

			foreach(var i in entities) {

				if(!i.gameObject.activeInHierarchy) continue;

				if(i.isFriendly==isFriendly) continue;
				float x = i.transform.position.x;
				float dist = Mathf.Abs(x-transform.position.x);

				if(dist<targetDistance) {
					targetDistance=dist;
					target=i;
				}

			}
			return target;
		}

		//若要攻击 则执行这个函数判断如何攻击
		public virtual ProjectileBase Attack(EntityBase target,int attackType) {

			if(additional.OverrideAttack(target,attackType)) return null;

			AudioController.PlayAudio(attacks[attackType].sound,transform.position);

			Vector3 position = transform.position;
			if(shootPosition) position=shootPosition.transform.position;
			Vector2 targetVelocity = Vector2.zero;
			if(target) targetVelocity=target.velocity;
			Vector3 targetPosition = transform.position+Vector3.right*Random.Range(-5,5);
			if(target) targetPosition=target.transform.position;

			return ProjectilePool.Create(
				attacks[attackType].projectile,
				position,
				ProjectileVelocity(targetPosition,targetVelocity,attackType),
				target,
				isFriendly,
				GetDamage(),
				attacks[attackType].manaGain
			);

		}

		//判断子弹的飞行速度 瞄准+预判
		protected bool projectileParametersGet;
		//计算射弹的发射速度
		protected virtual Vector2 ProjectileVelocity(Vector2 target,Vector2 velocity,int projectileType) {
			if(!projectileParametersGet) {
				projectileParametersGet=true;
				int cnt = attacks.Length;
				for(int i = 0;i<cnt;i++) attacks[i].InitParams();
			}

			AttackData attack = attacks[projectileType];

			float arriveX = target.x+Mathf.Clamp(velocity.x,-maxPredictSpeed,maxPredictSpeed)*attack.travelTime;
			float shootX = transform.position.x;
			if(shootPosition) shootX=shootPosition.position.x;

			float velocityX = (arriveX-shootX)/attack.travelTime;
			if(attack.travelTime==0) velocityX=0.1f*Direction.GetX(direction);
			return new Vector2(velocityX,attack.projectileVelocityY);

		}

		//让角色自行判断是否攻击
		public virtual void DoAttack(int type) {
			Attack(target,type);
		}

		#endregion

		public BuffSlot buffSlot;

		//用于判断动画状态转移
		protected int nameHash;
		protected bool nameHashSet;

	}


}