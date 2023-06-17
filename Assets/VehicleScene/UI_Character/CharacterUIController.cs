using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {

	public class CharacterUIController:MonoBehaviour {

		CharacterData currentCharacter;

		[SerializeField] List<CharacterInformationDisplay> displays;

		public void OnEnter(object param) {
			if(!(param is CharacterData)) {
				Home.HomeUIStackManager.instance.TryPopUI();
				return;
			}
			currentCharacter=param as CharacterData;
		}

		private void Update() {
			foreach(var i in displays) i.targetCharacter=currentCharacter;
		}

	}

}