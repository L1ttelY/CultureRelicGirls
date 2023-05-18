using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class WorkBenchController:BuildingWithCharacterInteractionBase {

		public static WorkBenchController instance { get; private set; }
		protected override void Start() {
			base.Start();
			instance=this;
		}

	}

}