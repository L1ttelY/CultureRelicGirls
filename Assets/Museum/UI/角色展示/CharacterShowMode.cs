using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterShowMode:UIModeBase {

		[SerializeField] BuildingLevelDisplay levelDisplay;
		[SerializeField] Text description;
		[SerializeField] Image levelUpCostButton;
		[SerializeField] Image levelUpTimeButton;
		[SerializeField] Image healCostButton;
		[SerializeField] Image healTimeButton;

		static CharacterShowMode instance;
		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		public bool infoPanelUp { get; private set; }

		int targetId;
		int targetLevel;
		CharacterController targetController;
		CharacterData targetStaticData;
		PlayerData.CharacterData targetSaveData;

		public static void EnterMode(CharacterController controller) {
			instance._EnterMode(controller);
		}
		void _EnterMode(CharacterController controller) {
			targetId=controller.characterIndex;
			targetController=controller;
			targetStaticData=controller.staticData;
			targetSaveData=PlayerData.PlayerDataRoot.instance.characterDatas[targetId];
			UIController.instance.SwitchUIMode(this);
			infoPanelUp=false;
		}

		private void Update() {
			targetLevel=targetSaveData.level.value;
			levelUpCostButton.color=targetController.CanLevelUpCost() ? Color.white : new Color(0.4f,0.4f,0.4f);
			levelUpTimeButton.color=targetController.CanLevelUpTime() ? Color.white : new Color(0.4f,0.4f,0.4f);
			healCostButton.color=targetController.CanHealCost() ? Color.white : new Color(0.4f,0.4f,0.4f);
			healTimeButton.color=targetController.CanHealTime() ? Color.white : new Color(0.4f,0.4f,0.4f);

			levelDisplay.level=targetLevel;
			description.text=targetStaticData.descriptionShort;

		}

		public void OnLevelUpCostButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanLevelUpCost()) {
				Message(CharacterController.messageBuffer);
				return;
			}
			string message =
				$"是否要花费{targetStaticData.levels[targetLevel].levelUpCost}点意识晶体\n"+
				$"并消耗{targetStaticData.levels[targetLevel].levelUpCostHour}时{targetStaticData.levels[targetLevel].levelUpCostMinute}分\n"+
				$"将{targetStaticData.name}从{targetLevel}级升级到{targetLevel+1}级?\n"+
				$"升级过程中角色仍可出战但不能修复, 升级过程无法打断";
			ConfirmationMode.EnterMode(message,() => { targetController.GoLevelUpCost(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void OnLevelUpTimeButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanLevelUpTime()) {
				Message(CharacterController.messageBuffer);
				return;
			}
			string message =
				$"是否要消耗{targetStaticData.levels[targetLevel].levelUpTimeHour}时{targetStaticData.levels[targetLevel].levelUpTimeMinute}分\n"+
				$"将{targetStaticData.name}从{targetLevel}级升级到{targetLevel+1}级?\n"+
				$"升级过程中角色仍可出战但不能修复, 升级过程无法打断";
			ConfirmationMode.EnterMode(message,() => { targetController.GoLevelUpTime(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void OnHealCostButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanHealCost()) {
				Message(CharacterController.messageBuffer);
				return;
			}

			string message =
				$"是否要花费{targetController.HealCost()}点意识晶体\n"+
				$"将{targetStaticData.name}完全修复?\n"+
				$"修复过程会很快完成";
			ConfirmationMode.EnterMode(message,() => { targetController.GoHealCost(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void OnHealTimeButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanHealTime()) {
				Message(CharacterController.messageBuffer);
				return;
			}

			System.TimeSpan healTime = targetController.HealTime();
			int h = (int)healTime.TotalHours;
			int m = healTime.Minutes;

			string message =
				$"是否消耗{h}时{m}分\n"+
				$"将{targetStaticData.name}完全修复?\n"+
				$"修复过程中无法出战, 可以随时中断";
			ConfirmationMode.EnterMode(message,() => { targetController.GoHealTime(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void Message(string message) {
			message+="\n点击是或否按键以继续游戏";
			ConfirmationMode.EnterMode(message,BackTothisMode,BackTothisMode);
		}
		public void BackTothisMode() {
			EnterMode(targetController);
		}

		public void OnInfoPanelClick() {
			infoPanelUp=!infoPanelUp;
		}

	}
}