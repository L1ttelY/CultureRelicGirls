using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {



	public class ListCoordinator:MonoBehaviour {

		ListElementController[] elements;

		private void Start() {
			elements=GetComponentsInChildren<ListElementController>();
			List<CharacterData> characters = new List<CharacterData>();
			foreach(var i in CharacterData.datas) {
				if(PlayerData.CharacterDataRoot.instance.characters[i.Value.name].level.value==0) continue;
				characters.Add(i.Value);
			}
			for(int i = 0;i<elements.Length;i++) {
				elements[i].Init(i,(i>=characters.Count) ? null : characters[i]);
			}

		}

	}

}