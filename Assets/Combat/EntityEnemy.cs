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

		#region 攻击目标

		protected override void UpdateTarget() {
			base.UpdateTarget();

			target=null;
			for(int i = targetIndex;i<3;i++) {
				if(EntityFriendly.friendlyList[i]) target=EntityFriendly.friendlyList[i];
			}

		}

		#endregion

		protected override float distanceToTarget => Mathf.Abs(transform.position.x-targetX);

		protected override void Start() {
			base.Start();
			direction=startRight ? Direction.right : Direction.left;
			StartInactive();

		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
		}

		//返回是否命中敌人
		protected virtual bool UpdateContactDamage() {
			//碰撞伤害
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

		protected override void StateAttack() {
			base.StateAttack();
			if(doingDamage) UpdateContactDamage();
		}

		#region 移动

		[Tooltip("单位选择移动时追求的与目标距离的最小值\n单位在进入移动状态时会在此值与最大值之间随机选择一个值作为追求的与目标距离, 在与目标的距离达到追求的距离后会进行一次状态转移")]
		[SerializeField] protected float targetDistanceMin;
		[Tooltip("单位选择移动时追求的与目标距离的最大值")]
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
				//向右	
				moveTargetX=targetX-targetDistance;
			} else {
				//向左
				moveTargetX=targetX+targetDistance;
			}

			//确定速度
			Vector2 targetVelocity = (moveTargetX>position.x ? Vector2.right : Vector2.left)*speedBuff*maxSpeed;
			direction=(targetX>position.x) ? Direction.right : Direction.left;
			float deltaSpeed = acceleration*((speedBuff+1)*0.5f)*Time.deltaTime;
			velocity=Vector2.MoveTowards(velocity,targetVelocity,deltaSpeed);

			position+=velocity*Time.deltaTime;
			transform.position=position;

			Debug.Log(position.x-moveTargetX);
			if(Mathf.Abs(position.x-moveTargetX)<0.2f){
				//结束移动
				Debug.Log("ATTACK!!!");
				StartRandomAttack();
			}

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