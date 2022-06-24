using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class BuildingLevelUpMode:UIModeBase {

		[SerializeField] GameObject extraButton;
		[SerializeField] Text descriptionDisplay;
		[SerializeField] BuildingLevelDisplay levelDisplay;
		[SerializeField] Image levelUpButtonImage;

		static BuildingLevelUpMode instance;
		public override void Init() {
			base.Init();
			if(instance) Debug.Log("Duplicate");
			instance=this;
		}

		int targetId;
		Void onExtraButtonClick;

		public static void EnterMode(int id,Void onExtraButtonClick) { instance._EnterMode(id,onExtraButtonClick); }

		void _EnterMode(int id,Void onExtraButtonClick) {

			targetId=id;
			this.onExtraButtonClick=onExtraButtonClick;
			UIController.instance.SwitchUIMode(this);
			extraButton.SetActive(onExtraButtonClick!=null);

		}

		public void ExtraButtonClick() {
			onExtraButtonClick?.Invoke();
		}

		public void LevelUpButtonClick() {
			if(!BuildingControllerBase.instances[targetId].CanLevelUp()) return;

			System.Text.StringBuilder @string = new System.Text.StringBuilder();
			@string.Append("是否要花费");
			@string.Append(BuildingControllerBase.instances[targetId].buildingData.levels[targetLevel].levelUpCostMaterial);
			@string.Append("单位碳材料来将\n");
			@string.Append(BuildingControllerBase.instances[targetId].buildingData.name);
			@string.Append("从");
			@string.Append(targetLevel);
			@string.Append("级升级到");
			@string.Append(targetLevel+1);
			@string.Append("级?");

			ConfirmationMode.EnterMode(@string.ToString(),OnLevelUpConfirm,OnLevelUpCancel);

		}

		void OnLevelUpConfirm() {
			BuildingControllerBase.instances[targetId].LevelUp();
			CameraController.instance.SetFocus(null);
		}
		void OnLevelUpCancel() {
			_EnterMode(targetId,onExtraButtonClick);
		}

		int targetLevel;
		private void Update() {

			targetLevel=PlayerData.PlayerDataRoot.instance.buildingDatas[targetId].level.value;
			descriptionDisplay.text=BuildingControllerBase.instances[targetId].buildingData.levels[targetLevel].description;
			levelDisplay.level=targetLevel;
			levelUpButtonImage.color=(BuildingControllerBase.instances[targetId].CanLevelUp()) ? Color.white : new Color(.4f,.4f,.4f,1);

		}

	}

}