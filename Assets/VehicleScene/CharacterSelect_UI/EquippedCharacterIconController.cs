using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Home;
using UnityEngine.EventSystems;

namespace VehicleScene {

	public class EquippedCharacterIconController:MonoBehaviour, IPointerClickHandler {

		static EquippedCharacterIconController[] instances = new EquippedCharacterIconController[3];

		[SerializeField] Image avatarImage;
		[SerializeField] int targetIndex;
		[SerializeField] Image costImage;
		[SerializeField] Sprite[] costSprites;

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

		void UpdateAvatarImage() {
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

		public void OnPointerClick(PointerEventData eventData) {
			targetCharacter="";
		}
	}

}