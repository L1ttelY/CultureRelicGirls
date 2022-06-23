using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class UIModeBase:MonoBehaviour {

		public void Init() {
			UIController.instance.AddMode(gameObject.name,this);
		}

	}
}