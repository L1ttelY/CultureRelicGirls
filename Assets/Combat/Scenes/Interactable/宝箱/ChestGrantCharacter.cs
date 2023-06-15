using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class ChestGrantCharacter:MonoBehaviour {

		[Tooltip("�򿪱���ʱ��õĽ�ɫ")]
		[SerializeField] CharacterData characterToGive;

		public void OnInteract() {
			var targetData = PlayerData.CharacterDataRoot.instance.characters[characterToGive];
			if(targetData.level.value==0) targetData.level.value=1;
		}

	}

}
