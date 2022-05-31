using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Fighter:EntityEnemy {

		protected override void StateMove() {

			Vector2 position = previousPosition;
			Vector2 targetVelocity = Vector2.left*speedBuff*maxSpeed;
			float deltaSpeed = acceleration*speedBuff*Time.deltaTime;
			velocity=Vector2.MoveTowards(velocity,targetVelocity,deltaSpeed);

			position+=velocity*Time.deltaTime;
			transform.position=position;
			previousPosition=position;


		}


	}

}