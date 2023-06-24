using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Home;
using UnityEngine.EventSystems;

namespace VehicleScene {

	public class EquippedCharacterIconController:MonoBehaviour, IPointerClickHandler {

		public static EquippedCharacterIconController[] instances = new EquippedCharacterIconController[3];

		[SerializeField] Image avatarImage;
		[SerializeField] int targetIndex;
		[SerializeField] Image costImage;
		[SerializeField] Sprite[] costSprites;
		[SerializeField] LastSelectCharacterController display;
		[SerializeField] GameObject equipButton;

		string targetCharacter {
			get => LoadoutController.teamMembers[targetIndex].value;
			set {
				LoadoutController.teamMembers[targetIndex].value=value;
				UpdateAvatarImage();
			}
		}

		Releasable releasable;

		private void Start() {
			instances[targetIndex]=this;
			releasable=GetComponent<Releasable>();
			UpdateAvatarImage();
		}
		private void Update() {
			int fromSlot = LastSelectCharacterController.instance.targetEquipSlot;
			CharacterData newData = LastSelectCharacterController.instance.lastPicked;

			if(newData&&newData.name!=targetCharacter&&fromSlot!=targetIndex)
				equipButton.SetActive(true);
			else equipButton.SetActive(false);
		}
		public void OnRelease(object content,Releasable from) {
			CharacterData character = content as CharacterData;
			if(!character) {
				from.releaseResult=false;
				return;
			}
			from.releaseResult=true;

			foreach(var i in instances)
				if(i.targetCharacter==character.name)
					i.targetCharacter="";
			targetCharacter=character.name;
		}

		public void UpdateAvatarImage() {
			if(targetCharacter.Length==0||!CharacterData.datas.ContainsKey(targetCharacter)) {
				avatarImage.color=Color.clear;
				costImage.sprite=costSprites[0];
			} else {
				CharacterData boundData = CharacterData.datas[targetCharacter];
				avatarImage.sprite=boundData.sprite;
				avatarImage.color=Color.white;
				int cost = Mathf.RoundToInt(boundData.combatPrefab.GetComponent<Combat.EntityFriendly>().skillCost/25);
				costImage.sprite=costSprites[cost];
			}
		}

		public void OnEquipCurrentClick() {
			CharacterData newData = LastSelectCharacterController.instance.lastPicked;

			LastSelectCharacterController.instance.targetEquipSlot=-1;

			for(int i = 0;i<3;i++)
				if(LoadoutController.GetTeamMember(i)==newData) {
					LoadoutController.SetTeamMember(i,null);
					instances[i].UpdateAvatarImage();
				}

			LoadoutController.SetTeamMember(targetIndex,newData);
			UpdateAvatarImage();
		}


		public void OnPointerClick(PointerEventData eventData) {

			if(targetCharacter.Length==0) return;
			display.SetTarget(CharacterData.datas[targetCharacter],targetIndex);
		}
	}

}