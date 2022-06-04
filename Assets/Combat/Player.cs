using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class Player:MonoBehaviour {

		[SerializeField] Transform leftBound;
		[SerializeField] Transform rightBound;

		RectTransform rect;

		public static Player instance;

		public float targetVelocity { get; private set; }

		void Start() {
			if(instance) Debug.Break();
			instance=this;

			rect=transform as RectTransform;
		}
		private void OnDestroy() {
			instance=null;
		}

		void Update() {
			targetVelocity=0;


		}

	}

}