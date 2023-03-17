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

		protected virtual float targetX {
			get {
				int targetIndex = 0;
				for(int i = targetIndex;i<3;i++) {
					if(EntityFriendly.friendlyList[i]) return EntityFriendly.friendlyList[i].transform.position.x;
				}
				return 0;
			}
		}

		protected override void Start() {
			base.Start();
			direction=startRight ? Direction.right : Direction.left;
			StartInactive();

		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
		}

		protected virtual void UpdateContactDamage() {
			//Åö×²ÉËº¦
			if(isKnockbacked) return;
			int cnt = collider.Cast(Vector2.left,Utility.raycastBuffer,Mathf.Abs(velocity.x)*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityFriendly other = hit.collider.GetComponent<EntityFriendly>();
				if(other) {
					DamageModel damage = GetDamage();

					if(other.isKnockbacked) damage.amount=0;

					damage.direction=Direction.left;
					damage.damageType=DamageType.Contact;
					other.Damage(damage);
				}
			}
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

		protected override void StateMove() {

			Vector2 position = transform.position;

			float targetX = this.targetX;
			Vector2 targetVelocity = (targetX>transform.position.x ? Vector2.right : Vector2.left)*speedBuff*maxSpeed;
			direction=(targetVelocity.x>0) ? Direction.right : Direction.left;

			if(Mathf.Abs(targetX-transform.position.x)<attackRangeMax) StartAttack();

			float deltaSpeed = acceleration*((speedBuff+1)*0.5f)*Time.deltaTime;
			velocity=Vector2.MoveTowards(velocity,targetVelocity,deltaSpeed);

			position+=velocity*Time.deltaTime;
			transform.position=position;

		}

		protected virtual void StartAttack() { currensState=StateAttack; }
		protected virtual void StateAttack() { }




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