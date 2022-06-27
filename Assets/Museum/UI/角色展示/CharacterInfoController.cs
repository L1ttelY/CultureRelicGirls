using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterInfoController:MonoBehaviour {

		CharacterSelectionMode owner;
		[SerializeField] Text nameText;
		[SerializeField] Text combatInfoText;
		[SerializeField] Text fullDescriptionText;
		[SerializeField] Image pictureImage;
		[SerializeField] GameObject up;
		[SerializeField] GameObject down;


	}

}