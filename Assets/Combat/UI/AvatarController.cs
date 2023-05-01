using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class AvatarController:MonoBehaviour {

		[SerializeField] Image cooldownImage;
		[SerializeField] Image cooldownBase;
		[SerializeField] Image hpImage;
		[SerializeField] Image hpBase;
		[SerializeField] Image avatarImage;
		[SerializeField] Image redCrossImage;
		[SerializeField] Text hpText;
		[SerializeField] Text powerText;
		[HideInInspector] public int targetIndex;

		bool inited;
		int previousHp = -1;
		int previousMaxHp = -1;
		int previousPower = -1;
		float previousCooldownAmount = -1;
		bool targetDead;

		EntityFriendly target;

		void Start() {
			avatarImage.color=Color.clear;
		}

		void Update() {

			if(!inited) {
				//init
				inited=true;

				target=EntityFriendly.friendlyList[targetIndex];

				if(!target) gameObject.SetActive(false);

				avatarImage.color=Color.white;
				avatarImage.sprite=target.GetComponentInChildren<SpriteRenderer>().sprite;

			}

			if(!targetDead&&!target) {

				//刚死亡
				redCrossImage.gameObject.SetActive(true);
				hpBase.gameObject.SetActive(false);
				hpText.gameObject.SetActive(false);
				powerText.gameObject.SetActive(false);
				cooldownBase.gameObject.SetActive(false);

				targetDead=true;

			} else if(!targetDead&&target) {

				//正常

				if(target.maxHp!=previousMaxHp||target.hp!=previousHp) {
					previousMaxHp=target.maxHp;
					previousHp=target.hp;
					hpText.text=previousHp+"/"+previousMaxHp;
					hpImage.fillAmount=(float)previousHp/(float)previousMaxHp;
				}

				int currentPower = Mathf.RoundToInt(target.attackBasePower*target.powerBuff);
				if(previousPower!=currentPower) {
					previousPower=currentPower;
					powerText.text="力量:"+currentPower;
				}

				float cooldownAmount = target.timeAfterAttack/target.attackCd;
				if(previousCooldownAmount!=cooldownAmount) {
					previousCooldownAmount=cooldownAmount;
					cooldownImage.fillAmount=previousCooldownAmount;
				}

			}

		}

	}

}