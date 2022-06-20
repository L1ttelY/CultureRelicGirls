using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class MenderController:MonoBehaviour {

		[SerializeField] BuildingController building;

		public static MenderController instance;
		private void Start() {
			if(instance) Debug.LogError("!!!Multiple Instance of Mender Controller!!!");
			instance=this;
		}

		private void FixedUpdate() {
			
		}

		public int levelUpMax { get { return building.saveData.level.value; } }

	}

}