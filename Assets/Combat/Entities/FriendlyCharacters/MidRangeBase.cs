using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class MidRangeBase:EntityFriendly {

		public override void Damage(DamageModel e) {
			if(e.damageType==DamageType.Ranged) return;
			base.Damage(e);
		}

	}

}