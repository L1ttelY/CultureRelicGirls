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

		//������ȴ���
		//��������CD����
		[SerializeField] protected float skillCd;
		//[SerializeField] protected float skill2Cd;
		[Tooltip("���߷�Χ")]
		[field: SerializeField] public float visionRange { get; protected set; }
		[Tooltip("����cd")]
		[field: SerializeField] public float attackCd { get; protected set; }
		[Tooltip("���й���������Ӧ������")]
		[SerializeField] protected List<FriendlyAttackData> attackMethods;

		//protected float skillCd;
		protected float timeAfterSkill;
		//��������CD��ɱ���
		public float skillCdProgress { get { return timeAfterSkill/skillCd; } }

		//����ʹ�÷�ʽ
		//���õȼ� ʹ�õ���������
		//������Ϸ��ʼʱ����
		//ϣ�����ݵȼ��ͼ�������ĳЩ�����Ļ���Ҫ��д
		public virtual void SetUse(CharacterUseModel use) {
			this.use=use;
			//if(use.actionSkillType!=0) Player.instance.skilledCharacter=this;
			//if(use.actionSkillType==1) skillCd=skill1Cd;
			//else if(use.actionSkillType==2) skillCd=skill2Cd;
		}

		//Ŀǰ��ʹ�÷�ʽ
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
		static void StaticUpdate() { //�������룿

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

		//���������Ҳ�Ľ�ɫλ�� ���ڵ��������λ��
		public static float leftestX { get; private set; }
		public static float rightestX { get; private set; }

		//�ڶ����е�λ��
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

			float buffedSpeed = maxSpeed*speedBuff; //�ƶ��ٶ�
			float buffedAcceleration = acceleration*speedBuff; //���ٶ�

			Vector2 position = transform.position; //λ��

			float targetVelocity;
			float deltaSpeed = buffedAcceleration*Time.deltaTime; //��λʱ���ٶ�

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

				//��
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

		#region ��������

		//��̿�ʼʱ����
		protected virtual void ChargeStart() {
			StartCharging();
		}
		//ʹ����������ʱ����
		public virtual void ActionSkill() {

			if(timeAfterSkill<skillCd) return;

			timeAfterSkill=0;
			animator.SetTrigger("attackStart");
			animator.SetFloat("attackType",-1);

		}
		//�ж������Ƿ��ڳ��
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

			Vector2 position = transform.position; //λ��
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

		#region ����
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