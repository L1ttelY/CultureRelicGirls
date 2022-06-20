using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public struct DamageModel {
		public int amount;          //攻击力
		public float knockback;     //击退力
		public EntityBase dealer;   //造成攻击者
		public int direction;       //攻击方向, 值的含义参考Direction类
	}

	public class EntityBase:MonoBehaviour {


		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		protected SpriteRenderer spriteRenderer;
		protected Animator animator;

		//属性 在prefab中编辑 不要动态修改
		[field: SerializeField] public float acceleration { get; protected set; }           //加速能力   
		[field: SerializeField] public float maxSpeed { get; protected set; }               //最大速率   
		[field: SerializeField] public int attackBasePower { get; protected set; }          //基础攻击力 
		[field: SerializeField] public float attackCd { get; protected set; }               //攻击间隔   
		[field: SerializeField] public float knockbackPower { get; protected set; }         //击退力
		[field: SerializeField] public int maxHp { get; protected set; }                    //最大生命值
		[field: SerializeField] public GameObject[] projectiles { get; protected set; }     //射弹种类 在数组中随机选取
		[field: SerializeField] public float attackRangeMin { get; protected set; }         //最小攻击距离
		[field: SerializeField] public float attackRangeMax { get; protected set; }         //最大攻击距离
		[field: SerializeField] public float projectileVelocityY { get; protected set; }    //投射物的纵向速率 越大则横向速率越慢越难以命中 若投射物重力为0则为投射物的水平速率
		[field: SerializeField] public float maxPredictSpeed { get; protected set; }        //最大预判速率 在预判攻击时若目标大于这个速率移动则是做以这个速率移动
		[field: SerializeField] public GameObject[] damageVfx { get; protected set; }       //伤害特效 在数组中随机选取

		//获取当前角色正常攻击的参数
		protected virtual DamageModel GetDamage() {
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

		//角色的朝向 参考Direction类
		protected int direction = Direction.right;

		//更新状态的事件 改变角色状态的代码请在关注这个事件的函数中调用
		public static event EventHandler UpdateStats;
		void OnUpdateStats() {
			powerBuff=1;
			knockbackBuff=1;
			cdSpeed=1;
			speedBuff=1;
			UpdateStats?.Invoke(this);
		}

		//对角色造成伤害
		public virtual void Damage(DamageModel e) {

			hp-=e.amount;
			StartKnockback(e.knockback,e.direction);

			int vfxIndex = Random.Range(0,damageVfx.Length);
			VfxPool.Create(damageVfx[vfxIndex],transform.position,e.direction);

			if(hp<=0) OnDeath();

		}

		//在死亡时触发的事件 想要在其他角色死亡时执行代码请关注这个事件
		public static event EventHandler Death;
		//死亡时触发的代码
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

		//移动相关
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

		//攻击
		public float timeAfterAttack { get; protected set; }
		//让角色自行判断是否攻击
		protected virtual void UpdateAttack() {

			timeAfterAttack+=Time.deltaTime;

			//找到敌人
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

		//若要攻击 则执行这个函数判断如何攻击
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

		//判断子弹的飞行速度 瞄准+预判
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