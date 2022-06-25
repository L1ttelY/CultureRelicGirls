using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterSelectionSlot:MonoBehaviour {

		int id;
		CharacterSelectionMode owner;
		[SerializeField] Image image;
		public void Init(int id) {
			this.id=id;
			owner=GetComponentInParent<CharacterSelectionMode>(true);
		}

		private void Update() {
			if(owner.characters[id]==-1) image.color=Color.clear;
			else {
				image.color=Color.white;
				image.sprite=owner.usePicture ? CharacterData.datas[owner.characters[id]].picture : CharacterData.datas[owner.characters[id]].sprite;
			}
		}

		public void OnClick() {
			owner.Choose(id);
		}

	}

}