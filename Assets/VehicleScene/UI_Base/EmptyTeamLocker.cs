using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {

	public class EmptyTeamLocker:MonoBehaviour {

		[SerializeField] GameObject[] targets;

		private void Update() {

			bool canGo = false;
			foreach(var i in LoadoutController.teamMembers) {
				if(i.value.Length!=0) {
					canGo=true;
					break;
				}
			}

			foreach(var i in targets) {
				if(!i) continue;
				i.SetActive(canGo);
			}

		}

	}

}