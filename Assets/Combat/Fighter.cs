using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Fighter:EntityEnemy {
		[SerializeField] float knockbackResistance=1;
		protected override void StartKnockback(float knockback,int direction) {
			base.StartKnockback(knockback*(1-knockbackResistance),direction);
		}

	}

}