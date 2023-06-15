using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {



	public class ListCoordinator:MonoBehaviour {

		ListElementController[] elements;

		private void Start() {
			elements=GetComponentsInChildren<ListElementController>();
			var currentCharacter = CharacterData.datas.GetEnumerator();
			for(int i = 0;i<elements.Length;i++) {
				bool end = !currentCharacter.MoveNext();
				elements[i].Init(i,end ? null : currentCharacter.Current.Value);
			}
			
		}

	}

}