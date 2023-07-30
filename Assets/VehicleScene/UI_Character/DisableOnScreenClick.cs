using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VehicleScene {

	public class DisableOnScreenClick:MonoBehaviour {

		float timeAfterEnable = 0;

		private void OnEnable() {
			timeAfterEnable=0;
		}

		private void Update() {
			timeAfterEnable+=Time.deltaTime;
			if(timeAfterEnable<0.1f) return;
			if(Input.GetMouseButtonDown(0)) gameObject.SetActive(false);
		}

	}

}