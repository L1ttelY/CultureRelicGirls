using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyDasher:EntityEnemy {

		float timeSinceAttack;
		int chargeSign;//-1×ó 1ÓÒ
		protected override void StartAttack() {
			base.StartAttack();
			timeSinceAttack=0;
			chargeSign=targetX>transform.position.x ? 1 : -1;
		}

		float chargeSpeed = 20;
		float chargeTime = 0.25f;
		float signatureTime = 0.6f;

		protected override void StateAttack() {
			base.StateAttack();
			timeSinceAttack+=Time.deltaTime;
			if(timeSinceAttack<signatureTime) {
				velocity=Vector2.zero;
				return;
			}

			velocity=Vector2.right*chargeSign*chargeSpeed;
			UpdateContactDamage();

			if(timeSinceAttack>signatureTime+chargeTime) StartMove();

		}

	}

}
