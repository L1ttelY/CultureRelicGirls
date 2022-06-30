using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class HatchStationController:BuildingControllerBase {

		[SerializeField] SpriteRenderer coverSprite;
		[SerializeField] SpriteRenderer contentSprite;
		CountDownController.CountDownToken hatchCountDown;

		static List<HatchStationController> hatchers = new List<HatchStationController>();

		const int statusNotHatching = -1;

		protected override void Start() {
			base.Start();
			hatchers.Add(this);
		}

		public override void OnClick(CameraFocus.CancelFocus cancelFocus) {

			if(saveData.level.value<0) {
				cancelFocus.doCancel=true;
				return;
			}

			bool canBeUsed = true;
			if(saveData.extraStatus.value>0) canBeUsed=false;
			if(saveData.level.value<=0) canBeUsed=false;

			if(canBeUsed) BuildingLevelUpMode.EnterMode(id,OnExtraButtonClick);
			else BuildingLevelUpMode.EnterMode(id,null);
			spriteRenderer.material=normalMaterial;
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
			if(saveData.extraStatus.value>0&&saveData.extraProgression.completion) {
				int characterId = saveData.extraStatus.value;
				saveData.extraStatus.value=statusNotHatching;

				int startLevel = saveData.level.value>=3 ? 2 : 1;
				PlayerData.PlayerDataRoot.instance.characterDatas[characterId].level.value=startLevel;
			}

			coverSprite.color=saveData.extraStatus.value>0 ? Color.clear : Color.white;
			if(saveData.extraStatus.value>0) contentSprite.sprite=CharacterData.datas[saveData.extraStatus.value].picture;

			if(saveData.extraStatus.value>0) {
				if(hatchCountDown==null) {
					hatchCountDown=CountDownController.instance.CreateCountDown();
				}
				hatchCountDown.boundObject.transform.position=contentSprite.transform.position+Vector3.up*0.5f;
				hatchCountDown.progressionImage.fillAmount=saveData.extraProgression.progressionAmount;
				hatchCountDown.textField.text=saveData.extraProgression.TimeLeftText();
			} else {
				if(hatchCountDown!=null) {
					CountDownController.instance.FreeCountDown(hatchCountDown);
					hatchCountDown=null;
				}

			}

			if(saveData.level.value<=0) {
				coverSprite.color=Color.clear;
				contentSprite.color=Color.clear;
			}

		}

		void OnExtraButtonClick() {
			CharacterSelectionMode.EnterMode(CharacterFilter,true,OnCharacterSelected);
		}
		void OnCharacterSelected(int id) {
			if(id<=0) {
				BuildingLevelUpMode.instance.BackToThisMode();
				return;
			}

			int cost = CharacterData.datas[id].levels[0].levelUpCost;
			if(PlayerData.PlayerDataRoot.smCount<cost) {
				//无法支付
				string message = $"需要{cost}意识晶体\n意识晶体不足\n按下是或否以继续游戏";
				ConfirmationMode.EnterMode(message,BuildingLevelUpMode.instance.BackToThisMode,BuildingLevelUpMode.instance.BackToThisMode);
			} else {
				//可以支付
				selectedCharacter=id;
				string message =
				$"是否消耗{cost}点意识晶体\n"+
				$"来意识化{CharacterData.datas[id].name}?\n";
				ConfirmationMode.EnterMode(message,OnConfirmHatch,BuildingLevelUpMode.instance.BackToThisMode);
			}

		}
		int selectedCharacter;
		void OnConfirmHatch() {
			PlayerData.PlayerDataRoot.smCount-=CharacterData.datas[selectedCharacter].levels[0].levelUpCost;
			saveData.extraStatus.value=selectedCharacter;
			float hatchTimeMultiplier = saveData.level.value>=2 ? 0.7f : 1;
			System.TimeSpan hatchtime = CharacterData.datas[selectedCharacter].levels[0].levelUpCostTime*hatchTimeMultiplier;
			saveData.extraProgression.SetProgression(hatchtime,0);
			CameraController.instance.SetFocus(null);
		}

		bool CharacterFilter(int id) {
			if(id==0) return false;
			int level = PlayerData.PlayerDataRoot.instance.characterDatas[id].level.value;
			if(level!=0) return false;
			if(IsCharacterHatching(id)) return false;
			return true;
		}

		bool IsCharacterHatching(int id) {
			foreach(var i in hatchers) {
				if(i.saveData.extraStatus.value==id) return true;
			}
			return false;
		}

	}

}
