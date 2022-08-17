using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class CharmTraditionalArmament:CharmBase {

		public CharmTraditionalArmament() {
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
		}

		public override void Terminate() {
			base.Terminate();
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
		}

		private void EntityBase_UpdateStats(object sender) {
			if(!(sender is EntityFriendly)) return;
			EntityFriendly _sender = sender as EntityFriendly;
			_sender.powerBuff+=0.03f;
		}

	}
}