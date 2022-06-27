using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class LevelStartCharacterSelection:MonoBehaviour {

		[SerializeField] int index;
		[SerializeField] Image hpBarImage;
		[SerializeField] GameObject infoRoot;
		[SerializeField] Image characterImage;
		[SerializeField] Text levelText;

		LevelStartMode owner;

		private void Start() {
			owner=GetComponentInParent<LevelStartMode>();
		}

		public void OnClick() {
			owner.OnSlotClick(index);
		}

		private void Update() {
			int characterIndex = owner.chosenCharacters[index];
			if(characterIndex<0) {
				//Î´Ñ¡Ôñ
				infoRoot.SetActive(false);
			} else {
				var characterData = PlayerData.PlayerDataRoot.instance.characterDatas[characterIndex];
				infoRoot.SetActive(true);
				hpBarImage.fillAmount=characterData.healthAmount;
				levelText.text=$"LV.{characterData.level.value}";
				characterImage.sprite=CharacterData.datas[characterIndex].sprite;
			}
		}

	}

}