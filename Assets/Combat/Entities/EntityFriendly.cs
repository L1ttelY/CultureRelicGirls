using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[System.Serializable]
	public class FriendlyAttackData {
		[HideInInspector] public int index;
		public float minDistance;
		public float maxDistance;
		public float weight;
	}

	public class EntityFriendly:EntityBase {

		//技能冷却相关
		//主动技能CD长度
		[Tooltip("视线范围")]
		[field: SerializeField] public float visionRange { get; protected set; }
		[Tooltip("攻击cd")]
		[field: SerializeField] public float attackCd { get; protected set; }
		[Tooltip("所有攻击动画对应的属性")]
		[SerializeField] protected List<FriendlyAttackData> attackMethods;
		[Tooltip("主动技能消耗的能量值")]
		[field: SerializeField] public float skillCost { get; protected set; }
		public Sprite icon { get; protected set; }

		//主动技能CD完成比例
		public float skillCdProgress { get { return Mathf.Clamp01(Player.instance.mana/skillCost); } }

		protected bool isBlocking;

		//设置使用方式
		//设置等级 使用的主动技能
		//会在游戏开始时调用
		//希望根据等级和技能设置某些变量的话需要重写
		public virtual void SetUse(CharacterUseModel use) {
			this.use=use;
			//if(use.actionSkillType!=0) Player.instance.skilledCharacter=this;
			//if(use.actionSkillType==1) skillCd=skill1Cd;
			//else if(use.actionSkillType==2) skillCd=skill2Cd;
		}

		//目前的使用方式
		protected CharacterUseModel use;

		protected override void Update() {

			hpList[name]=hp;

			if(Input.GetKey(KeyCode.X)&&positionInTeam!=0) transform.position+=Vector3.left*Time.deltaTime*10;

			base.Update();
			timeAfterAttack+=Time.deltaTime;

			bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
			isBlocking=Player.instance.isBlocking&&(!isAttacking);
			animator.SetBool("isBlocking",isBlocking);
		}

		static float[] nudgeWeights = new float[3];
		//static update
		[RuntimeInitializeOnLoadMethod]
		static void SubscribeStaticEvents() {
			EventManager.staticUpdate+=StaticUpdate;
		}
		public static readonly EntityFriendly[] sortedByX = new EntityFriendly[3];
		static void StaticUpdate() { //攻击距离？

			//计算友方单位范围
			float newLeftest = float.MaxValue;
			float newRightest = float.MinValue;
			float weightX = 0;
			int friendlyCount = 0;

			for(int i = 0;i<3;i++) {
				EntityFriendly a = friendlyList[i];
				if(!a) {
					sortedByX[i]=null;
					continue;
				}
				sortedByX[i]=a;
				newLeftest=Mathf.Min(a.transform.position.x,newLeftest);
				newRightest=Mathf.Max(a.transform.position.x,newRightest);
				friendlyCount++;
				weightX+=a.transform.position.x;
			}

			if(friendlyCount==0) return;

			weightX/=(float)friendlyCount;
			System.Array.Sort(sortedByX,(a,b) => {
				if(a==null&&b==null) return 0;
				if(a==null) return 1;
				if(b==null) return -1;
				return (int)Mathf.Sign(a.transform.position.x-b.transform.position.x);
			});

			const float maxDistance = 6;

			//互相凑近
			if(newRightest-newLeftest>maxDistance) {
				if(friendlyCount==2) {
					float nudgeTotal = (newRightest-newLeftest)-maxDistance;
					sortedByX[0].transform.position+=Vector3.right*(nudgeTotal/2f);
					sortedByX[1].transform.position+=Vector3.left*(nudgeTotal/2f);
				} else if(friendlyCount==3) {
					float center = 0;
					for(int i = 0;i<3;i++) center+=sortedByX[i].transform.position.x;
					center/=3f;
					for(int i = 0;i<3;i++) nudgeWeights[i]=center-sortedByX[i].transform.position.x;
					float nudgeTotal = (newRightest-newLeftest)-maxDistance;
					float weightTotal = Mathf.Abs(nudgeWeights[0])+Mathf.Abs(nudgeWeights[2]);

					for(int i = 0;i<3;i++) {
						float nudgeAmount = nudgeTotal/weightTotal*nudgeWeights[i];
						sortedByX[i].transform.position+=Vector3.right*nudgeAmount;
					}

				}

				newLeftest=float.MaxValue;
				newRightest=float.MinValue;

				foreach(var a in friendlyList) {
					newLeftest=Mathf.Min(a.transform.position.x,newLeftest);
					newRightest=Mathf.Max(a.transform.position.x,newRightest);
				}
			}

			leftestX=newLeftest;
			rightestX=newRightest;


		}

		public static bool HasTarget() {
			foreach(var i in friendlyList) {
				if(!i) continue;
				if(i.target) return true;
			}
			return false;
		}

		//最左侧和最右侧的角色位置 用于调整摄像机位置
		public static float leftestX { get; private set; }
		public static float rightestX { get; private set; }

		//在队伍中的位置
		[SerializeField] public int positionInTeam;

		public CharacterLevelData levelData { get; private set; }
		public void InitStats(CharacterLevelData data,int positionInTeam) {
			maxHp=data.hpMax;
			levelData=data;
			attackBasePower=data.power;
			this.positionInTeam=positionInTeam;
			icon=data.parent.sprite;
		}

		public static EntityFriendly playerControlled;
		public static List<EntityFriendly> friendlyList = new List<EntityFriendly>(3);
		public static readonly Dictionary<string,int> hpList = new Dictionary<string,int>();
		public static void RecoverAllHp() => hpList.Clear();

		public const float distancePerCharacter = 0.8f;
		public const float distanceTolerence = 0.1f;

		protected override void Start() {
			base.Start();
			if(positionInTeam==0) playerControlled=this;
			while(friendlyList.Count<3) friendlyList.Add(null);
			friendlyList[positionInTeam]=this;

			//Player.ActionSkillEvent+=Player_ActionSkillEvent;
			Player.ChargeEvent+=Player_ChargeEvent;
			CombatRoomController.RoomChange+=CombatRoomController_RoomChange;

			//初始化attackmethods
			for(int i = 0;i<attackMethods.Count;i++) {
				attackMethods[i].index=i;
			}

			if(hpList.ContainsKey(name)) hp=hpList[name];
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			//Player.ActionSkillEvent-=Player_ActionSkillEvent;
			Player.ChargeEvent-=Player_ChargeEvent;
			CombatRoomController.RoomChange-=CombatRoomController_RoomChange;
		}

		private void CombatRoomController_RoomChange(object _sender) {

			CombatRoomController sender = _sender as CombatRoomController;
			if(sender==null) return;

			transform.parent=sender.transform;
			transform.position=new Vector3(CombatController.startX,sender.transform.position.y);
			room=sender;

			Debug.Log(room.gameObject.name);

		}

		private void Player_ChargeEvent() {
			ChargeStart();
		}

		public float targetFinalPosition {
			get {

				EntityFriendly first = null;
				for(int comparisonIndex = positionInTeam-1;comparisonIndex>=0;comparisonIndex--) {
					if(friendlyList[comparisonIndex]) first=friendlyList[comparisonIndex];
				}
				if(!first) return transform.position.x;

				float offsetPerCharacter = distancePerCharacter*(Player.instance.teamDirection==Direction.left ? 1 : -1);
				return first.transform.position.x+(positionInTeam-first.positionInTeam)*offsetPerCharacter;

			}
		}

		protected override void StateMove() {
			base.StateMove();

			velocity.y=0;

			float buffedSpeed = maxSpeed*speedBuff; //移动速度
			float buffedAcceleration = acceleration*speedBuff; //加速度

			Vector2 position = transform.position; //位置

			float targetVelocity;
			float deltaSpeed = buffedAcceleration*Time.deltaTime; //单位时间速度

			int previousIndex = -1;
			EntityFriendly previousEntity = null;
			for(int comparisonIndex = positionInTeam-1;comparisonIndex>=0;comparisonIndex--) {
				if(friendlyList[comparisonIndex]) {
					previousEntity=friendlyList[comparisonIndex];
					previousIndex=comparisonIndex;
					break;
				}
			}


			if(!previousEntity) {
				targetVelocity=buffedSpeed*Player.instance.targetVelocity;

			} else {

				float targetPosition = previousEntity.transform.position.x;
				targetPosition+=(positionInTeam-previousIndex)*distancePerCharacter*(Player.instance.teamDirection==Direction.left ? 1 : -1);

				float decelerateDistance = 0.5f*velocity.x*velocity.x/buffedAcceleration;
				if(decelerateDistance<distanceTolerence) decelerateDistance=distanceTolerence;
				targetVelocity=buffedSpeed*(targetPosition>position.x ? 1 : -1);

				//起步
				float distance = Mathf.Abs(targetPosition-position.x);
				if(distance<decelerateDistance) targetVelocity=0;


			}

			if(Mathf.Abs(targetVelocity-velocity.x)<deltaSpeed) velocity.x=targetVelocity;
			else if(targetVelocity<velocity.x) velocity.x-=deltaSpeed;
			else velocity.x+=deltaSpeed;

			position.x+=velocity.x*Time.deltaTime;
			position.y=room.transform.position.y;
			transform.position=position;

			if(target) direction=(target.transform.position.x>transform.position.x) ? Direction.right : Direction.left;
			else direction=Player.instance.teamDirection;
			UpdateAttack();

		}

		protected override void OnDeath() {
			base.OnDeath();

			FriendlyCorpseController.Create(transform,spriteRenderer.sprite);
			CameraController.instance.AddScreenShake(10);

			Destroy(gameObject);
		}

		protected override void UpdateTarget() {
			target=null;

			bool targetAttackable = false;
			float targetDistance = float.MaxValue;

			foreach(var i in entities) {
				if(!i.gameObject.activeInHierarchy) continue;
				if(i.isFriendly) continue;
				float dist = Mathf.Abs(transform.position.x-i.transform.position.x);
				if(dist>visionRange) continue;
				bool attackable = false;
				foreach(var attack in attackMethods) {
					if(dist<attack.maxDistance&&dist>attack.minDistance) {
						attackable=true;
						break;
					}
				}

				if(targetAttackable) {
					if(!attackable) continue;
					if(dist<targetDistance) {
						targetDistance=dist;
						target=i;
					}
				} else {
					if(dist<targetDistance) {
						targetDistance=dist;
						target=i;
						targetAttackable=attackable;
					}
				}

			}

		}


		#region 主动技能

		//冲刺开始时调用
		protected virtual void ChargeStart() {
			StartCharging();
		}
		//使用主动技能时调用
		public virtual bool TryUseActionSkill() {

			if(Player.instance.mana<skillCost) return false;
			Player.instance.mana-=skillCost;

			animator.SetTrigger("attackStart");
			animator.SetFloat("attackType",-1);
			return true;

		}
		//判断自身是否在冲刺
		public static bool isAnyoneCharging {
			get {
				foreach(var i in friendlyList) {
					if(!i) continue;
					if(i.isCharging) return true;
				}
				return false;
			}
		}
		public bool isCharging { get { return currensState==StateCharging; } }
		[SerializeField] protected float chargeTime = 0.5f;
		protected float timeCharged;

		void StartCharging() {
			currensState=StateCharging;
			timeCharged=0;
		}
		[SerializeField] protected AnimationCurve chargeSpeedCurve;
		[SerializeField] protected float chargeSpeedMax;
		void StateCharging() {

			animator.SetBool("isCharging",true);

			timeCharged+=Time.deltaTime;

			Vector2 position = transform.position; //位置
			velocity.y=0;
			velocity.x=chargeSpeedCurve.Evaluate(timeCharged/chargeTime)*Player.instance.chargeDirection*chargeSpeedMax;

			position.x+=velocity.x*Time.deltaTime;
			position.y=room.transform.position.y;

			transform.position=position;

			if(timeCharged>chargeTime) {

				animator.SetBool("isCharging",false);
				StartMove();
			}
		}
		#endregion

		#region 攻击
		protected virtual void UpdateAttack() {
			if(isBlocking) return;
			if(timeAfterAttack<attackCd) return;
			if(target==null) return;
			float dist = Mathf.Abs(target.transform.position.x-transform.position.x);
			var viableAttacks = attackMethods.FindAll((FriendlyAttackData a) => { return dist<a.maxDistance&&dist>a.minDistance; });
			if(viableAttacks.Count==0) return;
			int attackIndex = viableAttacks[ChooseByWeight.Work((int a) => viableAttacks[a].weight,viableAttacks.Count)].index;

			animator.SetTrigger("attackStart");
			animator.SetFloat("attackType",attackIndex);
			timeAfterAttack=0;

		}

		#endregion

		public static DamageModel friendlyLastDamage { get; protected set; }

		public override void Damage(DamageModel e) {
			if(Player.instance.UseParry()) {
				if(e.damageType!=DamageType.Ranged&&e.dealer)
					e.dealer.DoKnockback(30,Direction.Reverse(e.direction));
				return;
			}
			if(isBlocking) {
				e.amount/=2;
				if(e.damageType!=DamageType.Ranged&&e.dealer)
					e.dealer.DoKnockback(8,Direction.Reverse(e.direction));
			}
			base.Damage(e);
		}

	}
}