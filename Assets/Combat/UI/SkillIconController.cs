using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class SkillIconController:MonoBehaviour {

		[SerializeField] int targetIndex;
		[SerializeField] Image cooldownGray;
		[SerializeField] Image costIndicator;
		[SerializeField] Sprite[] costSprites;
		[SerializeField] Image iconImage;

		bool inited = false;
		EntityFriendly target;

		void Update() {

			if(!inited) {
				inited=true;
				target=EntityFriendly.friendlyList[targetIndex];
				if(!target) {
					gameObject.SetActive(false);
					return;
				}
				iconImage.sprite=target.GetComponent<EntityFriendly>().icon;
				costIndicator.sprite=costSprites[(int)((target.skillCost+1)/25)];
			}

			if(!target) {
				cooldownGray.fillAmount=1;
			} else {
				cooldownGray.fillAmount=1-target.skillCdProgress;
			}

		}
	}

}
