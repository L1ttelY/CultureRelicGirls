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
		public DamageType damageType;          //伤害类型
		public int amount;                     //攻击力
		public float knockback;                //击退力
		public EntityBase dealer;              //造成攻击者
		public ProjectileBase dealerProjectile;//造成攻击的射弹
		public int direction;                  //攻击方向, 值的含义参考Direction类
	}

	public class EntityBase:MonoBehaviour {

		public static LinkedList<EntityBase> entities = new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//component reference
		protected new Collider2D collider;
		protected SpriteRenderer spriteRenderer;
		protected Animator animator;

		protected CombatRoomController room;

		#region 属性 在prefab中编辑 不要动态修改
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

		[SerializeField] protected AudioClip soundAttack;
		[SerializeField] protected AudioClip soundHit;
		[SerializeField] protected AudioClip soundDeath;

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
			UpdateStats?.Invoke(this);

		}

		public DamageModel lastDamage { get; protected set; }            //最后一次受到的伤害
		public static event System.EventHandler<DamageModel> DamageEvent;//受到伤害时触发的事件


		//对角色造成伤害
		public virtual void Damage(DamageModel e) { //受到伤害
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

		//移动相关
		protected Vector2 velocity;
		virtual protected void UpdateMove() {
			Vector2 position = transform.position;
			position+=velocity*Time.deltaTime;
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

		#region 攻击目标

		protected virtual void UpdateTarget() { target=GetNearestTarget(); }

		protected EntityBase target;

		protected virtual float targetX => target==null ? transform.position.x : target.transform.position.x;

		protected virtual float distanceToTarget => Mathf.Abs(targetX-transform.position.x);

		#endregion

		#region 攻击动画/攻击状态

		[SerializeField] protected float attackChance = 1;        //移动结束后进入攻击状态的概率
		[SerializeField] protected AttackStateData[] attacks;     //包含所有攻击状态的列表
		public float timeAfterAttack { get; protected set; }
		//让角色自行判断是否攻击
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

		//播放攻击动画

		public float 攻击动画移动速度;
		protected float overrideSpeed {
			get => 攻击动画移动速度;
			set => 攻击动画移动速度=value;
		}

		protected int attackIndex;

		protected readonly static SortedSet<AttackStateData> attackStatesBuffer = new SortedSet<AttackStateData>();
		protected readonly static SortedSet<AttackStateTransistion> transitionBuffer = new SortedSet<AttackStateTransistion>();

		protected virtual void StartRandomAttack() {
			float weightTotal = 0;
			float distanceToTarget = this.distanceToTarget;

			//找出可能的转移目标
			attackStatesBuffer.Clear();
			foreach(var i in attacks) {
				if(i.maxDistance<distanceToTarget||i.minDistance>distanceToTarget) continue;
				weightTotal+=i.startWeight;
				attackStatesBuffer.Add(i);
			}

			//选择实际的转移目标(攻击动画)
			float randomFactor = Random.Range(0,weightTotal);
			AttackStateData targetState = null;
			foreach(var i in attackStatesBuffer) {
				randomFactor-=i.startWeight;
				if(randomFactor<=Mathf.Epsilon) {
					targetState=i;
					break;
				}
			}

			//判断要转移到攻击动画还是行走
			if(Utility.Chance(1-attackChance)||targetState==null) {
				//继续行走
				StartMove();
			} else {
				//进行攻击
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

		//在代码中提前结束攻击的接口
		protected virtual void EndAttack() {
			animator.SetTrigger("attackEnd");
			StartMove();
		}

		//通过动画在攻击结束时调用
		public virtual void 攻击事件_AttackEnd() {
			transitionBuffer.Clear();
			float distanceToTarget = this.distanceToTarget;
			AttackStateData currentAttack = attacks[attackIndex];
			float weightTotal = 0;

			//统计可用的目标状态
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

			//判断最终目标
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

		#region 默认攻击事件


		//获取应该攻击的敌人
		protected virtual EntityBase GetNearestTarget() {
			//找到敌人
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

		//若要攻击 则执行这个函数判断如何攻击
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

		//判断子弹的飞行速度 瞄准+预判
		protected bool projectileParametersGet;
		protected float[] projectileGravity;
		protected float[] travelTime;
		//计算射弹的发射速度
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