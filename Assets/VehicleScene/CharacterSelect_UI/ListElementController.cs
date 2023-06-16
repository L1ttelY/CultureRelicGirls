using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Home;

namespace VehicleScene {

	public class ListElementController:MonoBehaviour {

		[SerializeField] Image avatarImage;

		Draggable draggable;

		int index;
		CharacterData boundCharacter;

		private void Start() {
			draggable=GetComponent<Draggable>();
		}

		public void Init(int index,CharacterData boundCharacter) {
			this.index=index;
			this.boundCharacter=boundCharacter;
			if(boundCharacter==null) {
				avatarImage.sprite=null;
				avatarImage.color=Color.clear;
			} else {
				avatarImage.sprite=boundCharacter.sprite;
			}
		}

		private void Update() {
			draggable.content=boundCharacter;
			if(boundCharacter)
				draggable.sprite=boundCharacter.sprite;
		}

	}

}