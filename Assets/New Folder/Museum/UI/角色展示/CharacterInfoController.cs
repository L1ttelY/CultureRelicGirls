using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterInfoController:MonoBehaviour {

		CharacterShowMode owner;
		[SerializeField] Text nameText;
		[SerializeField] Text combatInfoText;
		[SerializeField] Text fullDescriptionText;
		[SerializeField] Image pictureImage;
		[SerializeField] Image buttonImage;
		[SerializeField] Sprite upButton;
		[SerializeField] Sprite downButton;
		[SerializeField] Transform upPosition;
		[SerializeField] Transform downPosition;

		private void Start() {
			owner=GetComponentInParent<CharacterShowMode>();
		}

		float previousHp;
		int previousId;
		int previousLevel = -1;

		private void Update() {

			if(owner.targetLevel!=previousLevel||owner.targetId!=previousId||owner.targetSaveData.healthAmount!=previousHp) {

				//更新信息
				previousLevel=owner.targetLevel;
				previousId=owner.targetId;
				previousHp=owner.targetSaveData.healthAmount;

				int maxHp = owner.targetStaticData.levels[owner.targetLevel].hpMax;
				combatInfoText.text=
				$"血量:{Mathf.FloorToInt(owner.targetSaveData.healthAmount*maxHp)}/{maxHp}\n"+
				$"力量:{owner.targetStaticData.levels[owner.targetLevel].power}\n"+
				owner.targetStaticData.skillDescription;

				nameText.text=owner.targetStaticData.name;
				fullDescriptionText.text=owner.targetStaticData.fullDescription;
				pictureImage.sprite=owner.targetStaticData.picture;

			}

			Vector3 targetPosition = Vector3.zero;
			if(owner.infoPanelUp) {
				buttonImage.sprite=downButton;
				targetPosition=upPosition.position;
			} else {
				buttonImage.sprite=upButton;
				targetPosition=downPosition.position;
			}

			Vector3 velocity = targetPosition-transform.position;
			transform.position+=velocity*10*Time.deltaTime;

		}

		private void OnDisable() {
			transform.position=downPosition.position;
		}

	}

}