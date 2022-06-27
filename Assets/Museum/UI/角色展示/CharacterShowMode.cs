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
				$"�Ƿ�Ҫ����{targetStaticData.levels[targetLevel].levelUpCost}����ʶ����\n"+
				$"������{targetStaticData.levels[targetLevel].levelUpCostHour}ʱ{targetStaticData.levels[targetLevel].levelUpCostMinute}��\n"+
				$"��{targetStaticData.name}��{targetLevel}��������{targetLevel+1}��?\n"+
				$"���������н�ɫ�Կɳ�ս�������޸�, ���������޷����";
			ConfirmationMode.EnterMode(message,() => { targetController.GoLevelUpCost(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void OnLevelUpTimeButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanLevelUpTime()) {
				Message(CharacterController.messageBuffer);
				return;
			}
			string message =
				$"�Ƿ�Ҫ����{targetStaticData.levels[targetLevel].levelUpTimeHour}ʱ{targetStaticData.levels[targetLevel].levelUpTimeMinute}��\n"+
				$"��{targetStaticData.name}��{targetLevel}��������{targetLevel+1}��?\n"+
				$"���������н�ɫ�Կɳ�ս�������޸�, ���������޷����";
			ConfirmationMode.EnterMode(message,() => { targetController.GoLevelUpTime(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void OnHealCostButtonClick() {
			if(infoPanelUp) return;
			if(!targetController.CanHealCost()) {
				Message(CharacterController.messageBuffer);
				return;
			}

			string message =
				$"�Ƿ�Ҫ����{targetController.HealCost()}����ʶ����\n"+
				$"��{targetStaticData.name}��ȫ�޸�?\n"+
				$"�޸����̻�ܿ����";
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
				$"�Ƿ�����{h}ʱ{m}��\n"+
				$"��{targetStaticData.name}��ȫ�޸�?\n"+
				$"�޸��������޷���ս, ������ʱ�ж�";
			ConfirmationMode.EnterMode(message,() => { targetController.GoHealTime(); CameraController.instance.SetFocus(null); },BackTothisMode);
		}

		public void Message(string message) {
			message+="\n����ǻ�񰴼��Լ�����Ϸ";
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