using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {


	public class UIController:MonoBehaviour {
		[SerializeField] bool hideUI;
		void Start() {
			GetComponentInChildren<CombatRewardUIController>(true).Init();
		}

		private void LateUpdate() {
			if(hideUI) {
				var list = GetComponentsInChildren<Graphic>(true);
				foreach(var i in list) i.color=Color.clear;
			}
		}

	}
}