using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityEnemy:EntityBase {

		[field: SerializeField] public int enemyId { get; private set; }
		[SerializeField] protected float wakeUpDistanceFront;
		[SerializeField] protected float wakeUpDistanceBack;
		[SerializeField] Sprite corpseSprite;
		[SerializeField] int sentienceMatterReward;

		[SerializeField] bool startRight;

		[SerializeField] protected bool doingDamage;
		[SerializeField] Collider2D damageBox;

		[SerializeField] int targetIndex;

		#region ����Ŀ��

		protected override void UpdateTarget() {
			base.UpdateTarget();

			target=null;
			for(int i = targetIndex;i<3;i++) {
				if(EntityFriendly.friendlyList[i]) target=EntityFriendly.friendlyList[i];
			}

		}

		#endregion
		#region ��������/����״̬

		[SerializeField] protected float attackChance = 1;        //�ƶ���������빥��״̬�ĸ���
		[SerializeField] protected AttackStateData[] attacks;     //�������й���״̬���б�


		//���Ź�������

		public float ���������ƶ��ٶ�;
		protected float overrideSpeed {
			get => ���������ƶ��ٶ�;
			set => ���������ƶ��ٶ�=value;
		}

		protected int attackIndex;

		protected readonly static List<AttackStateData> attackStatesBuffer = new List<AttackStateData>();
		protected readonly static List<AttackStateTransistion> transitionBuffer = new List<AttackStateTransistion>();

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
			int targetIndex = ChooseByWeight.Work((int a) => attackStatesBuffer[a].startWeight,attackStatesBuffer.Count);
			AttackStateData targetState = targetIndex<0 ? null : attackStatesBuffer[targetIndex];

			/*
			float randomFactor = Random.Range(0,weightTotal);
			foreach(var i in attackStatesBuffer) {
				randomFactor-=i.startWeight;
				if(randomFactor<=Mathf.Epsilon) {
					targetState=i;
					break;
				}
			}
			*/

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
			animator.SetFloat($"attackType",attackIndex);
			animator.SetTrigger($"attackStart");

			//animator.SetTrigger($"attack{attackIndex}");
			currensState=StateAttack;
			nameHashSet=false;
		}
		protected virtual void StateAttack() {

			AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
			if(!nameHashSet) {
				nameHashSet=true;
				nameHash=state.fullPathHash;
			}
			if(nameHash!=state.fullPathHash) AttackEnd();

			velocity=overrideSpeed*Direction.GetVector(direction);
			overrideSpeed=0;
			if(doingDamage) UpdateContactDamage();

		}

		//�ڴ�������ǰ���������Ľӿ�
		protected virtual void EndAttackPremature() {
			animator.SetTrigger("attackEnd");
			StartMove();
		}

		//ͨ�������ڹ�������ʱ����
		public virtual void AttackEnd() {
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
			int targetIndex = ChooseByWeight.Work((a) => transitionBuffer[a].weight,transitionBuffer.Count);
			AttackStateTransistion transistion = transitionBuffer[targetIndex];
			/*
			float randomFactor = Random.Range(0,weightTotal);
			foreach(var i in transitionBuffer) {
				randomFactor-=i.weight;
				if(randomFactor<=Mathf.Epsilon) {
					transistion=i;
					break;
				}
			}
			*/

			if(transistion==null||transistion.type==AttackStateTransistionType.Move) StartMove();
			else StartAttack(transistion.attackId);

		}
		#endregion

		protected override float distanceToTarget => Mathf.Abs(transform.position.x-targetX);

		protected override void Start() {
			base.Start();
			direction=startRight ? Direction.right : Direction.left;
			StartInactive();
			poise=poiseMax;
			for(int i = 0;i<attacks.Length;i++) attacks[i].id=i;
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
			poise+=poiseRegen*Time.deltaTime;
			if(poise>poiseMax) poise=poiseMax;
			if(poise<=0) StartStagger();
		}

		//�����Ƿ����е���
		protected virtual bool UpdateContactDamage() {
			//��ײ�˺�
			if(isKnockbacked) return false;

			bool result = false;

			int cnt = damageBox.Cast(Vector2.left,Utility.raycastBuffer,Mathf.Abs(velocity.x)*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityFriendly other = hit.collider.GetComponent<EntityFriendly>();
				if(other) {
					DamageModel damage = GetDamage();

					if(other.isKnockbacked) damage.amount=0;

					damage.direction=direction;
					damage.damageType=DamageType.Contact;
					other.Damage(damage);
					result=true;
				}
			}

			return result;
		}

		protected virtual void StartInactive() {
			currensState=StateInactive;
		}
		protected virtual void StateInactive() {
			float x = transform.position.x;
			bool toActive = false;

			float sightLeft = x-(direction==Direction.right ? wakeUpDistanceBack : wakeUpDistanceFront);
			float sightRight = x+(direction==Direction.left ? wakeUpDistanceBack : wakeUpDistanceFront);

			foreach(var i in EntityFriendly.friendlyList) {
				if(!i) continue;
				float pos = i.transform.position.x;
				if(pos<sightRight&&pos>sightLeft) toActive=true;
			}

			if(toActive) StartMove();
		}

		#region �ƶ�

		[Tooltip("��λѡ���ƶ�ʱ׷�����Ŀ��������Сֵ\n��λ�ڽ����ƶ�״̬ʱ���ڴ�ֵ�����ֵ֮�����ѡ��һ��ֵ��Ϊ׷�����Ŀ�����, ����Ŀ��ľ���ﵽ׷��ľ��������һ��״̬ת��")]
		[SerializeField] protected float targetDistanceMin;
		[Tooltip("��λѡ���ƶ�ʱ׷�����Ŀ���������ֵ")]
		[SerializeField] protected float targetDistanceMax;

		protected float targetDistance;

		protected override void StartMove() {
			base.StartMove();
			targetDistance=Random.Range(targetDistanceMin,targetDistanceMax);
		}

		protected override void StateMove() {

			Vector2 position = transform.position;
			float moveTargetX;
			if(targetX>position.x) {
				//����	
				moveTargetX=targetX-targetDistance;
			} else {
				//����
				moveTargetX=targetX+targetDistance;
			}

			//ȷ���ٶ�
			Vector2 targetVelocity = (moveTargetX>position.x ? Vector2.right : Vector2.left)*speedBuff*maxSpeed;
			direction=(targetX>position.x) ? Direction.right : Direction.left;
			float deltaSpeed = acceleration*((speedBuff+1)*0.5f)*Time.deltaTime;
			velocity=Vector2.MoveTowards(velocity,targetVelocity,deltaSpeed);

			position+=velocity*Time.deltaTime;
			transform.position=position;

			if(Mathf.Abs(position.x-moveTargetX)<0.2f) {
				//�����ƶ�
				StartRandomAttack();
			}

		}

		public void �ƶ��¼�_EndMove() {
			StartRandomAttack();
		}

		#endregion

		#region ���Ի���

		[Tooltip("���ܻ���ʱ�ܵ��Ļ������������˺��ᱻ��ȥ���ֵ")]
		[SerializeField] protected float knockbackDefense;
		[Tooltip("�������ֵ")]
		[SerializeField] protected float poiseMax;
		[Tooltip("����ֵ�ָ�����")]
		[SerializeField] protected float poiseRegen;

		protected float poise;
		protected float staggeredTime;

		protected override void DoKnockback(float knockback,int direction) {
			float actualKnockback = Mathf.Max(0,knockback-knockbackDefense);
			poise-=actualKnockback;
			base.DoKnockback(knockback,direction);
		}

		protected virtual void StartStagger() {
			currensState=StateStagger;
			animator.SetTrigger("stagger");
			nameHashSet=false;
		}
		protected virtual void StateStagger() {
			var state = animator.GetCurrentAnimatorStateInfo(0);
			if(!nameHashSet) {
				nameHash=state.fullPathHash;
				nameHashSet=true;
			}

			velocity=Vector2.zero;
			poise=poiseMax;
			if(state.fullPathHash!=nameHash) StartMove();

		}

		#endregion

		protected override void OnDeath() {
			base.OnDeath();
			if(corpseSprite!=null) {
				EnemyCorpse newCorpse = EnemyCorpse.Create(corpseSprite,transform.position,direction);
				newCorpse.gameObject.transform.parent=room.transform;
				CombatController.instance.rewardSm+=sentienceMatterReward;
				Destroy(gameObject);
			}
		}

	}

}