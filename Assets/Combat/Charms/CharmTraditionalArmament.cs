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
			if(!(sender is EntityBase)) return;
			EntityBase _sender = sender as EntityBase;
			if(!_sender.isFriendly) return;
			_sender.powerBuff+=0.03f;
		}

	}
}