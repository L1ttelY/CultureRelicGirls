using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Home;

namespace VehicleScene {

	public class EquippedCharacterIconController:MonoBehaviour {

		[SerializeField]

		Releasable releasable;

		private void Start() {
			releasable=GetComponent<Releasable>();
		}

		public void OnRelease(object content,Releasable from) {
			CharacterData character = content as CharacterData;
			if(!character) {
				from.releaseResult=false;
				return;
			}
			from.releaseResult=true;



		}


	}

}