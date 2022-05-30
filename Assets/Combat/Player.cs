using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Player:MonoBehaviour {

		public static Player instance;

		public float targetVelocity { get; private set; }

		void Start() {
			if(instance) Debug.Break();
			instance=this;
		}
		private void OnDestroy() {
			instance=null;
		}

		void Update() {
			targetVelocity=0;
			if(Input.GetKey(KeyCode.D)) targetVelocity+=1;
			if(Input.GetKey(KeyCode.A)) targetVelocity-=1;
		}

	}

}