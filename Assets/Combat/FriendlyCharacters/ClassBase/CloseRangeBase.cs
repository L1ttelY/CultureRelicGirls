using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class CloseRangeBase:EntityFriendly {

		public override void Damage(DamageModel e) {
			if(isCharging) return;
			base.Damage(e);
		}

	}

}