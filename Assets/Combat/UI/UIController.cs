using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class UIController:MonoBehaviour {
		void Start() {
			GetComponentInChildren<CombatRewardUIController>(true).Init();
		}
	}
}