using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityFriendly:EntityBase {

		//技能冷却相关
		//主动技能CD长度
		[SerializeField] protected float skill1Cd;
		[SerializeField] protected float skill2Cd;
		protected float skillCd;
		protected float timeAfterSkill;
		//主动技能CD完成比例
		public float skillCdProgress { get { return skillCd/timeAfterSkill; } }

		public virtual void SetUse(CharacterUseModel use) {
			this.use=use;
			if(use.actionSkillType!=0) Player.instance.skilledCharacter=this;
			if(use.actionSkillType==1) skillCd=skill1Cd;
			else if(use.actionSkillType==2) skillCd=skill2Cd;
		}
		
		protected CharacterUseModel use;

		protected override void Update() {
			base.Update();
			timeAfterSkill+=Time.deltaTime;
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

		public static float leftestX { get; private set; }
		public static float rightestX { get; private set; }

		[SerializeField] public int positionInTeam;

		public void InitStats(int hp,int power,int positionInTeam) {
			maxHp=hp;
			attackBasePower=power;
			this.positionInTeam=positionInTeam;
		}

		public static EntityFriendly playerControlled;
		public static List<EntityFriendly> friendlyList = new List<EntityFriendly>();

		const float distancePerCharacter = 1;
		const float distanceTolerence = 0.3f;

		protected override void Start() {
			base.Start();
			if(positionInTeam==0) playerControlled=this;	
			while(friendlyList.Count<=positionInTeam) friendlyList.Add(null);
			friendlyList[positionInTeam]=this;

			Player.ActionSkillEvent+=Player_ActionSkillEvent;
			Player.ChargeEvent+=Player_ChargeEvent;
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			Player.ActionSkillEvent-=Player_ActionSkillEvent;
			Player.ChargeEvent-=Player_ChargeEvent;
		}

		private void Player_ChargeEvent() {
			ChargeStart();
		}
		private void Player_ActionSkillEvent() {
			if(use.actionSkillType==1) ActionSkill1();
			else if(use.actionSkillType==2) ActionSkill2();
		}

		protected override void StateMove() {
			base.StateMove();

			velocity.y=0;

			float buffedSpeed = maxSpeed*speedBuff; //移动速度
			float buffedAcceleration = acceleration*speedBuff; //加速度

			Vector2 position = previousPosition; //位置

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

				float targetPosition = 0;
				targetPosition=previousEntity.transform.position.x-(positionInTeam-previousIndex)*distancePerCharacter;

				float decelerateDistance = 0.5f*velocity.x*velocity.x/buffedAcceleration;
				if(decelerateDistance<distanceTolerence) decelerateDistance=distanceTolerence;
				targetVelocity=buffedSpeed*(targetPosition>position.x ? 1 : -1);
				if(Mathf.Abs(targetPosition-position.x)<decelerateDistance) {
					float distance = Mathf.Abs(targetPosition-position.x);

					targetVelocity=0;
					if(distance<distanceTolerence) targetVelocity=0;

					if(targetVelocity>buffedSpeed) targetVelocity=buffedSpeed;
					targetVelocity=targetVelocity*(targetPosition>position.x ? 1 : -1);
				}

			}

			if(Mathf.Abs(targetVelocity-velocity.x)<deltaSpeed) velocity.x=targetVelocity;
			else if(targetVelocity<velocity.x) velocity.x-=deltaSpeed;
			else velocity.x+=deltaSpeed;

			position.x+=velocity.x*Time.deltaTime;
			position.y=0;
			transform.position=position;
			previousPosition=position;

			UpdateAttack();

		}

		protected override void OnDeath() {
			base.OnDeath();

			FriendlyCorpseController.Create(transform,spriteRenderer.sprite);

			Destroy(gameObject);
		}

		protected virtual void ChargeStart() {
			StartCharging();
		}
		protected virtual void ActionSkill1() { }
		protected virtual void ActionSkill2() { }

		protected bool isCharging { get { return currensState==StateCharging; } }
		protected const float chargeTime = 0.3f;
		protected float timeCharged;

		void StartCharging() {
			currensState=StateCharging;
			timeCharged=0;
		}
		const float startChargeSpeed = 15;
		const float endChargeSpeed = 5;
		void StateCharging() {

			timeCharged+=Time.deltaTime;

			Vector2 position = previousPosition; //位置
			velocity.y=0;
			velocity.x=Mathf.Lerp(startChargeSpeed,endChargeSpeed,timeCharged/chargeTime)*Player.instance.chargeDirection;

			position.x+=velocity.x*Time.deltaTime;
			position.y=0;
			transform.position=position;
			previousPosition=position;

			if(timeCharged>chargeTime) StartMove();

		}

	}
}