using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public struct DamageModel {
		public int amount;
		public float knockback;
		public EntityBase dealer;
		public int direction;
	}

	public class EntityBase:MonoBehaviour {

		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		protected SpriteRenderer spriteRenderer;
		protected Animator animator;

		//属性
		[field: SerializeField] public float acceleration { get; protected set; }
		[field: SerializeField] public float maxSpeed { get; protected set; }
		[field: SerializeField] public int attackBasePower { get; protected set; }
		[field: SerializeField] public float attackCd { get; protected set; }
		[field: SerializeField] public float knockbackPower { get; protected set; }
		[field: SerializeField] public int maxHp { get; protected set; }
		[field: SerializeField] public GameObject[] projectiles { get; protected set; }
		[field: SerializeField] public float attackRangeMin { get; protected set; }
		[field: SerializeField] public float attackRangeMax { get; protected set; }
		[field: SerializeField] public float projectileVelocityY { get; protected set; }
		[field: SerializeField] public float maxPredictSpeed { get; protected set; }
		[field: SerializeField] public bool useSword { get; protected set; }
		[field: SerializeField] public GameObject[] damageVfx { get; protected set; }

		protected virtual DamageModel GetDamage() {
			DamageModel result = new DamageModel();
			result.amount=Mathf.RoundToInt(attackBasePower*powerBuff);
			result.knockback=knockbackPower*powerBuff;
			result.dealer=this;
			return result;
		}

		//当前状态
		public int hp { get; protected set; }
		[HideInInspector] public float powerBuff;
		[HideInInspector] public float cdSpeed;
		[HideInInspector] public float speedBuff;

		protected int direction = Direction.right;

		public static event EventHandler UpdateStats;
		void OnUpdateStats() {
			powerBuff=1;
			cdSpeed=1;
			speedBuff=1;
			UpdateStats?.Invoke(this);
		}

		public virtual void Damage(DamageModel e) {
			hp-=e.amount;
			StartKnockback(e.knockback,e.direction);

			int vfxIndex = Random.Range(0,damageVfx.Length);
			VfxPool.Create(damageVfx[vfxIndex],transform.position,e.direction);

			if(hp<=0) OnDeath();
		}

		public static event EventHandler Death;
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
				}

			}

			float arriveX = target.x+Mathf.Clamp(velocity.x,-maxPredictSpeed,maxPredictSpeed)*travelTime[projectileType];

			if(useSword) return (target.x>transform.position.x ? Vector2.right : Vector2.left)*0.5f;
			float velocityX = (arriveX-transform.position.x)/travelTime[projectileType];
			return new Vector2(velocityX,projectileVelocityY);


		}

	}



}