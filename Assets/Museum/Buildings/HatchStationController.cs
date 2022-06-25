using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class HatchStationController:BuildingControllerBase {

		[SerializeField] SpriteRenderer coverSprite;
		[SerializeField] SpriteRenderer contentSprite;

		static List<HatchStationController> hatchers = new List<HatchStationController>();

		const int statusNotHatching = -1;

		protected override void Start() {
			base.Start();
			hatchers.Add(this);
		}

		public override void OnClick(CameraFocus.CancelFocus cancelFocus) {

			bool canBeUsed = true;
			if(saveData.extraStatus.value>0) canBeUsed=false;
			if(saveData.levelUpStatus.value!=0) canBeUsed=false;

			if(canBeUsed) BuildingLevelUpMode.EnterMode(id,OnExtraButtonClick);
			else BuildingLevelUpMode.EnterMode(id,null);
			spriteRenderer.material=normalMaterial;
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
			if(saveData.extraStatus.value>0&&saveData.extraProgression.completion) {
				int characterId = saveData.extraStatus.value;	
				saveData.extraStatus.value=statusNotHatching;
				PlayerData.PlayerDataRoot.instance.characterDatas[characterId].level.value=1;
			}

			coverSprite.color=saveData.extraStatus.value>0 ? Color.clear : Color.white;
			if(saveData.extraStatus.value>0) contentSprite.sprite=CharacterData.datas[saveData.extraStatus.value].picture;

		}

		public override bool CanLevelUp() {
			if(!base.CanLevelUp()) return false;
			if(saveData.extraStatus.value>0) {
				messageBuffer="��������ʹ����";
				return false;
			}
			return true;
		}

		void OnExtraButtonClick() {
			CharacterSelectionMode.EnterMode(CharacterFilter,true,OnCharacterSelected);
		}
		void OnCharacterSelected(int id) {

			Debug.Log(id);

			int cost = CharacterData.datas[id].levels[0].levelUpCost;
			if(PlayerData.PlayerDataRoot.smCount<cost) {
				//�޷�֧��
				string message = $"��Ҫ{cost}��ʶ����\n��ʶ���岻��\n�����ǻ���Լ�����Ϸ";
				ConfirmationMode.EnterMode(message,BuildingLevelUpMode.instance.BackToThisMode,BuildingLevelUpMode.instance.BackToThisMode);
			} else {
				//����֧��
				selectedCharacter=id;
				string message =
				$"�Ƿ�����{cost}����ʶ����\n"+
				$"����ʶ��{CharacterData.datas[id].name}?\n";
				ConfirmationMode.EnterMode(message,OnConfirmHatch,BuildingLevelUpMode.instance.BackToThisMode);
			}

		}
		int selectedCharacter;
		void OnConfirmHatch() {
			PlayerData.PlayerDataRoot.smCount-=CharacterData.datas[selectedCharacter].levels[0].levelUpCost;
			saveData.extraStatus.value=selectedCharacter;
			saveData.extraProgression.SetProgression(CharacterData.datas[selectedCharacter].levels[0].levelUpCostTime,0);
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
