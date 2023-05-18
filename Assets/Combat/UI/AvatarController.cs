using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class AvatarController:MonoBehaviour {

		[SerializeField] Image cooldownImage;
		[SerializeField] Image hpImage;
		[SerializeField] Image hpBase;
		[SerializeField] Image avatarImage;
		[SerializeField] Image redCrossImage;
		[SerializeField] Text hpText;
		[SerializeField] Text powerText;
		[SerializeField] Text nameText;
		[SerializeField] Material monocrhomeMaterial;
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
				avatarImage.sprite=target.GetComponent<EntityFriendly>().icon;
				nameText.text=target.levelData.parent.name;

			}

			if(!targetDead&&!target) {

				//刚死亡
				hpText.text=$"生命值:  0/{previousMaxHp}";
				avatarImage.material=monocrhomeMaterial;
				targetDead=true;
				hpImage.fillAmount = 0;

			} else if(!targetDead&&target) {

				//正常

				if(target.maxHp!=previousMaxHp||target.hp!=previousHp) {
					previousMaxHp=target.maxHp;
					previousHp=target.hp;
					hpText.text= $"生命值:  {previousHp}/{previousMaxHp}";
					hpImage.fillAmount=(float)previousHp/(float)previousMaxHp;
				}

				int currentPower = Mathf.RoundToInt(target.attackBasePower*target.powerBuff);
				if(previousPower!=currentPower) {
					previousPower=currentPower;
					powerText.text="攻击力:  "+currentPower;
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