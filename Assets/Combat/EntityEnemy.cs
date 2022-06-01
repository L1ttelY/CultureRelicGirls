using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityEnemy:EntityBase {

		[SerializeField] protected float wakeUpDistance;

		protected override void Start() {
			base.Start();
			StartIdle();
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();

			//Åö×²ÉËº¦
			int cnt = collider.Cast(Vector2.left,Utility.raycastBuffer,Mathf.Abs(velocity.x)*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityFriendly other = hit.collider.GetComponent<EntityFriendly>();
				if(other) {
					DamageModel damage = GetDamage();
					damage.direction=Direction.left;
					other.Damage(damage);
				}
			}

		}

		protected virtual void StartIdle() {
			currensState=StateIdle;
		}

		protected virtual void StateIdle() {
			float x = transform.position.x;
			bool toActive = false;
			if(x<=EntityFriendly.rightestX+wakeUpDistance&&x>=EntityFriendly.leftestX-wakeUpDistance) toActive=true;
			if(toActive) StartMove();
		}


		protected override void StateMove() {

			Vector2 position = previousPosition;


			Vector2 targetVelocity = Vector2.left*speedBuff*maxSpeed;
			if(position.x<EntityFriendly.rightestX+attackRangeMax) targetVelocity=Vector2.zero;
			if(position.x<EntityFriendly.rightestX+attackRangeMin) targetVelocity=-Vector2.left*speedBuff*maxSpeed;


			float deltaSpeed = acceleration*speedBuff*Time.deltaTime;
			velocity=Vector2.MoveTowards(velocity,targetVelocity,deltaSpeed);

			position+=velocity*Time.deltaTime;
			transform.position=position;
			previousPosition=position;

			UpdateAttack();
		}

		protected override void OnDeath() {
			base.OnDeath();
			Destroy(gameObject);
		}

	}

}