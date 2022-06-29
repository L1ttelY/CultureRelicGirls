using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyJumper:EntityEnemy {

		[SerializeField] float kbResistance = 0.5f;

		protected override void StartKnockback(float knockback,int direction) {
			Vector2 kbVector = Direction.GetVector(direction)*knockback*(1-kbResistance);
			transform.position+=(Vector3)kbVector;
			previousPosition+=kbVector;
		}

		protected override void Update() {
			base.Update();
			animator.SetFloat("speed",currensState==StateJump ? 10 : 0);
			animator.SetBool("inKnockback",false);
		}

		float timeStayed;
		float timeToStay;
		protected override void StartMove() {
			base.StartMove();
			timeStayed=0;
			timeToStay=Random.Range(0.5f,1);

		}
		protected override void StateMove() {
			velocity=Vector2.zero;
			previousPosition=transform.position;
			timeStayed+=Time.deltaTime;
			float distance = 0;
			if(timeStayed>timeToStay) {
				direction=Direction.right;
				foreach(var i in EntityFriendly.friendlyList) {
					if(!i) continue;
					if (i.transform.position.x < transform.position.x)
					{
						direction = Direction.left;
						distance = i.transform.position.x - this.transform.position.x;
					}
				}
				if (direction == Direction.left)
					StartJump(Random.Range(7f, 8f), direction);
				else if (distance>6f)
					StartJump(distance+5f, direction);
				else
					StartJump(Random.Range(4f, 5f), direction);


			}

		}

		[SerializeField] float jumpTime = 0.35f;
		[SerializeField] float jumpHeight = 5f;

		float timeSinceJump;
		float jumpDistance;
		int jumpDirection;
		void StartJump(float jumpDistance,int jumpDirection) {
			timeSinceJump=0;
			currensState=StateJump;
			this.jumpDirection=jumpDirection;
			this.jumpDistance=jumpDistance;
		}
		void StateJump() {
			timeSinceJump+=Time.deltaTime;
			Vector2 position = previousPosition;

			float curveX = (timeSinceJump-0.5f*jumpTime)/jumpTime;
			position.y=jumpHeight*(-curveX*curveX+0.25f);
			position+=Direction.GetVector(jumpDirection)*Time.deltaTime*jumpDistance/jumpTime;


			curveX=(timeSinceJump+Time.deltaTime-0.5f*jumpTime)/jumpTime;
			float nextY = jumpHeight*(-curveX*curveX+0.25f);

			velocity=Direction.GetVector(jumpDirection)*jumpDistance/jumpTime;
			velocity.y=(nextY-position.y)/Time.deltaTime;

			transform.position=position;
			previousPosition=position;
			direction=jumpDirection;

			if(timeSinceJump>=jumpTime) {
				StartMove();

				foreach(var i in EntityFriendly.friendlyList) {
					if(!i) continue;
					if(Mathf.Abs(transform.position.x-i.transform.position.x)<attackRangeMax) {
						i.Damage(GetDamage());
					}
				}

			}
		}

		protected override void UpdateContactDamage() { }

	}

}