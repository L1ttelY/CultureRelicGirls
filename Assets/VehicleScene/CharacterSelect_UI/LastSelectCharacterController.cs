using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleScene {

	public class LastSelectCharacterController:MonoBehaviour {

		public static LastSelectCharacterController instance { get; private set; }

		public CharacterData lastPicked;
		public int targetEquipSlot;

		[SerializeField] CharacterInformationDisplay display;
		[SerializeField] GameObject displayRoot;
		[SerializeField] GameObject unequipObject;
		[SerializeField] Home.HomeUIInstance detailMode;

		public void SetTarget(CharacterData lastPicked,int targetEquipSlot) {
			this.lastPicked=lastPicked;
			this.targetEquipSlot=targetEquipSlot;
		}

		public void OnDetailClick() {
			if(lastPicked==null) return;
			Home.HomeUIStackManager.instance.PushUI((detailMode, lastPicked));
		}
		public void OnUnequipClick() {
			if(lastPicked==null) return;
			if(targetEquipSlot<0||targetEquipSlot>2) return;
			LoadoutController.teamMembers[targetEquipSlot].value="";
			foreach(var i in EquippedCharacterIconController.instances) i.UpdateAvatarImage();
		}

		private void OnEnable() {
			targetEquipSlot=-1;
			instance=this;
		}

		private void Update() {

			if(lastPicked) {
				displayRoot.SetActive(true);
				display.targetCharacter=lastPicked;
				unequipObject.SetActive(targetEquipSlot>=0&&targetEquipSlot<=2);
			} else {
				displayRoot.SetActive(false);
			}

		}

	}

}