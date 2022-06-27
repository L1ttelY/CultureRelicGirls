using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterSelectionSlot:MonoBehaviour {

		int id;
		CharacterSelectionMode owner;
		[SerializeField] Image image;
		[SerializeField] Image hpBarImage;
		[SerializeField] GameObject infoRoot;
		[SerializeField] Text levelDisplay;
		public void Init(int id) {
			this.id=id;
			owner=GetComponentInParent<CharacterSelectionMode>(true);
		}

		private void Update() {
			if(owner.characters[id]==-1) {
				image.color=Color.clear;
				infoRoot.SetActive(false);
			} else {
				image.color=Color.white;
				image.sprite=owner.usePicture ? CharacterData.datas[owner.characters[id]].picture : CharacterData.datas[owner.characters[id]].sprite;
				PlayerData.CharacterData saveData = PlayerData.PlayerDataRoot.instance.characterDatas[owner.characters[id]];
				if(saveData.level.value>0){
					infoRoot.SetActive(true);
					hpBarImage.fillAmount=saveData.healthAmount;
					levelDisplay.text=$"LV.{saveData.level.value}";
				}else infoRoot.SetActive(false);
			}
		}

		public void OnClick() {
			owner.Choose(id);
		}

	}

}