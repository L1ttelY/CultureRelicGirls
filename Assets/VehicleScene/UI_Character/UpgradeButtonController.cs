using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VehicleScene {

	public class UpgradeButtonController:MonoBehaviour {

		[SerializeField] GameObject displayObject;
		[SerializeField] GameObject upgradeObject;

		[SerializeField] Text costText;
		Image image;

		bool inUpgrade;

		private void Start() {
			image=GetComponent<Image>();
			VehicleStaticUIController.OnReturn+=VehicleStaticUIController_OnReturn;
		}
		private void OnDestroy() {
			VehicleStaticUIController.OnReturn-=VehicleStaticUIController_OnReturn;
		}
		private void VehicleStaticUIController_OnReturn(object sender) {
			if(inUpgrade) {
				var from = sender as VehicleStaticUIController;
				from.stopPop=true;
				inUpgrade=false;
				image.color=Color.white;

				displayObject.SetActive(true);
				upgradeObject.SetActive(false);

			}
		}

		private void OnEnable() {
			image=GetComponent<Image>();
			displayObject.SetActive(true);
			upgradeObject.SetActive(false);
			inUpgrade=false;
			image.color=Color.white;
		}

		bool canUpgrade;

		void UpdateUpgradeStatus() {
			CharacterData boundCharacter = upgradeObject.GetComponent<CharacterInformationDisplay>().targetCharacter;
			PlayerData.CharacterData boundSaveData = PlayerData.CharacterDataRoot.instance.characters[boundCharacter.name];

			if(boundSaveData.level.value>=boundCharacter.maxLevel) {
				costText.text="已经满级";
				canUpgrade=false;
			} else {
				string currentSmText = PlayerData.PlayerDataRoot.smCount.ToString();
				int levelUpCost = boundCharacter.levels[boundSaveData.level.value].levelUpCost;
				canUpgrade=PlayerData.PlayerDataRoot.smCount>=levelUpCost;

				if(!canUpgrade) currentSmText=$"<color=red>{currentSmText}</color>";
				costText.text=$"消耗意识晶体:   {currentSmText} / {levelUpCost}";
			}

			image.color=canUpgrade ? Color.white : Color.gray;

		}

		public void OnClick() {

			if(!inUpgrade) {

				inUpgrade=true;
				displayObject.SetActive(false);
				upgradeObject.SetActive(true);
				UpdateUpgradeStatus();


			} else {

				if(canUpgrade) {
					CharacterData boundCharacter = upgradeObject.GetComponent<CharacterInformationDisplay>().targetCharacter;
					PlayerData.CharacterData boundSaveData = PlayerData.CharacterDataRoot.instance.characters[boundCharacter.name];

					PlayerData.PlayerDataRoot.smCount-=boundCharacter.levels[boundSaveData.level.value].levelUpCost;
					boundSaveData.level.value+=1;
					UpdateUpgradeStatus();

				}

			}

		}

	}

}