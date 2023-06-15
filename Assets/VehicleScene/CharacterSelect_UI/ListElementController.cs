using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VehicleScene {

  public class ListElementController:MonoBehaviour {

		[SerializeField] Image avatarImage;

    int index;
    CharacterData boundCharacter;
		public void Init(int index,CharacterData boundCharacter) {
			this.index=index;
			this.boundCharacter=boundCharacter;
			avatarImage.sprite=boundCharacter.sprite;
		}



	}

}