using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

  public class CameraController:MonoBehaviour {

    public static CameraController instance;
		private void Start() {
			instance=this;
		}

		public GameObject focus;

	}

}