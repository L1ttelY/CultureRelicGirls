using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Home;

namespace VehicleScene {

	public class SwitchUIButton:MonoBehaviour {

		[SerializeField] HomeUIInstance targetUI;

		public void OnClick() {

			HomeUIStackManager.instance.PushUI((targetUI, null));

		}

	}

}
