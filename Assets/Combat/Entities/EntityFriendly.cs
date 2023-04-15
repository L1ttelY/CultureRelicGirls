using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[System.Serializable]
	public class FriendlyAttackData {
		public float minDistance;
		public float maxDistance;
		public float weight;
	}

	public class EntityFriendly:EntityBase {

		//技能冷却相关
		//主动技能CD长度
		[SerializeField] protected float skillCd;
		//[SerializeField] protected float skill2Cd;
		[Tooltip("视线范围")]
		[field: SerializeField] public float visionRange { get; protected set; }
		[Tooltip("攻击cd")]
		[field: SerializeField] public float attackCd { get; protected set; }
		[Tooltip("所有攻击动画对应的属性")]
		[SerializeField] protected List<FriendlyAttackData> attackMethods;

		//protected float skillCd;
		protected float timeAfterSkill;
		//主动技能CD完成比例
		public float skillCdProgress { get { return timeAfterSkill/skillCd; } }

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
			base.Update();
			timeAfterSkill+=Time.deltaTime;
			timeAfterAttack+=Time.deltaTime;
		}

		//static update
		[RuntimeInitializeOnLoadMethod]
		static void SubscribeStaticEvents() {
			EventManager.staticUpdate+=StaticUpdate;
		}
		static void StaticUpdate() { //攻击距离？

			float originalLeftest = leftestX;
			float originalRightest = rightestX;
			bool friendlyLeft = false;

			leftestX=float.MaxValue;
			rightestX=float.MinValue;

			foreach(var i in entities) {
				if(i is EntityFriendly) {

					leftestX=Mathf.Min(i.transform.position.x,leftestX);
					rightestX=Mathf.Min(i.transform.position.x,leftestX);
					friendlyLeft=true;

				}
			}

			if(!friendlyLeft) {
				leftestX=originalLeftest;
				rightestX=originalRightest;
			}

		}

		//最左侧和最右侧的角色位置 用于调整摄像机位置
		public static float leftestX { get; private set; }
		public static float rightestX { get; private set; }

		//在队伍中的位置
		[SerializeField] public int positionInTeam;

		public void InitStats(int hp,int power,int positionInTeam) {
			maxHp=hp;
			attackBasePower=power;
			this.positionInTeam=positionInTeam;
		}

		public static EntityFriendly playerControlled;
		public static List<EntityFriendly> friendlyList = new List<EntityFriendly>();

		const float distancePerCharacter = 0.8f;
		const float distanceTolerence = 0.1f;

		protected override void Start() {
			base.Start();
			if(positionInTeam==0) playerControlled=this;
			while(friendlyList.Count<=positionInTeam) friendlyList.Add(null);
			friendlyList[positionInTeam]=this;

			//Player.ActionSkillEvent+=Player_ActionSkillEvent;
			Player.ChargeEvent+=Player_ChargeEvent;
			CombatRoomController.RoomChange+=CombatRoomController_RoomChange;
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
				if(distance<distanceTolerence) targetVelocity=0;


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

			Destroy(gameObject);
		}

		protected override void UpdateTarget() {
			target=null;

			bool targetAttackable = false;
			float targetDistance = float.MaxValue;

			foreach(var i in entities) {
				if(!i.gameObject.activeInHierarchy) continue;
				if(i is EntityFriendly) continue;
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
		public virtual void ActionSkill() {

			if(timeAfterSkill<skillCd) return;

			timeAfterSkill=0;
			animator.SetTrigger("attackStart");
			animator.SetFloat("attackType",-1);

		}
		//判断自身是否在冲刺
		protected bool isCharging { get { return currensState==StateCharging; } }
		protected const float chargeTime = 0.5f;
		protected float timeCharged;

		void StartCharging() {
			currensState=StateCharging;
			timeCharged=0;
		}
		const float startChargeSpeed = 25;
		const float endChargeSpeed = 5;
		void StateCharging() {

			animator.SetBool("isCharging",true);

			timeCharged+=Time.deltaTime;

			Vector2 position = transform.position; //位置
			velocity.y=0;
			velocity.x=Mathf.Lerp(startChargeSpeed,endChargeSpeed,timeCharged/chargeTime)*Player.instance.chargeDirection;

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
			if(timeAfterAttack<attackCd) return;
			if(target==null) return;
			float dist = Mathf.Abs(target.transform.position.x-transform.position.x);
			var viableAttacks = attackMethods.FindAll((FriendlyAttackData a) => { return dist<a.maxDistance&&dist>a.minDistance; });
			if(viableAttacks.Count==0) return;
			int attackIndex = ChooseByWeight.Work((int a) => viableAttacks[a].weight,viableAttacks.Count);

			animator.SetTrigger("attackStart");
			animator.SetFloat("attackType",attackIndex);
			timeAfterAttack=0;

		}

		#endregion

	}
}