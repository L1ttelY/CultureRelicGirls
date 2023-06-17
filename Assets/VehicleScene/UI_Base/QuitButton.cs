using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {


	public class QuitButton:MonoBehaviour {

		private void Start() {
			if(!Combat.StationController.lastStationVisited) Destroy(gameObject);
		}

		public void OnClick() {
			Combat.CombatController.StartCombat(Combat.StationController.lastStationVisited);
		}

	}

}