using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class ResearchStationController:BuildingWithCharacterInteractionBase {

		public static ResearchStationController instance { get; private set; }
		protected override void Start() {
			base.Start();
			instance=this;
		}

	}

}