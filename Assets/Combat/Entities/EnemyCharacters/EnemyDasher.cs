using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyDasher:EntityEnemy {

		float timeSinceAttack;
		int chargeSign;//-1×ó 1ÓÒ

		protected override bool UpdateContactDamage() {
			bool result = base.UpdateContactDamage();
			//if(result&&currensState==StateAttack) EndAttack();
			return result;
		}

	}

}