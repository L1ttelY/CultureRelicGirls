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
			animator.SetTrigger("attack");
		}

		float moveSpeed = 3;
		float chargeSpeed = 10;
		float signatureEnd = 0.6f;
		float preChargeEnd = 1.1f;
		float chargeEnd = 1.7f;
		float animationEnd = 2.1f;

		protected override void StateAttack() {
			base.StateAttack();
			timeSinceAttack+=Time.deltaTime;
			if(timeSinceAttack<signatureEnd) {
				velocity=Vector2.zero;
			}else if(timeSinceAttack<preChargeEnd){
				velocity=Vector2.right*chargeSign*moveSpeed;
			}else if(timeSinceAttack<chargeEnd){
				velocity=Vector2.right*chargeSign*chargeSpeed;
				UpdateContactDamage();
			}else if(timeSinceAttack<animationEnd){
				velocity=Vector2.right*chargeSign*moveSpeed;
			} else StartMove();

			animator.ResetTrigger("hit");

		}

	}

}
