using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class GuillotineAdditional:EntityAdditionalFunctionBase {

		[Tooltip("技能持续时间")]
		[SerializeField] float duration;
		[Tooltip("每秒钟损失血量占总血量的比例")]
		[SerializeField] float hpLossRate;
		[Tooltip("吸血倍率")]
		[SerializeField] float hpRecoveryRate;
		[Tooltip("能量回复速率")]
		[SerializeField] float manaRegenRate;

		float timeRemaining;

		float maxMana;
		float hpLossBuildup;

		private void Start() {
			EntityBase.DamageEvent+=EntityBase_DamageEvent;
		}

		private void EntityBase_DamageEvent(object sender,DamageModel e) {
			if(e.dealer==entity) {

				foreach(var i in EntityFriendly.friendlyList) {
					if(!i) continue;
					i.Heal((int)(e.amount*hpRecoveryRate));
				}
			}
		}

		private void OnDestroy() {
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
		}

		private void Update() {

			//技能激活
			timeRemaining-=Time.deltaTime;
			if(timeRemaining>0) {

				if(Player.instance.mana<maxMana) {
					Player.instance.mana+=Time.deltaTime*manaRegenRate;
					if(Player.instance.mana>maxMana) Player.instance.mana=maxMana;
				}

				hpLossBuildup+=Time.deltaTime*hpLossRate;
				while(hpLossBuildup>=0.02f) {
					hpLossBuildup-=0.02f;
					DamageModel hpReduction = new DamageModel();
					hpReduction.damageType=DamageType.HpLoss;

					foreach(var i in EntityFriendly.friendlyList) {
						if(!i) continue;
						hpReduction.amount=Mathf.FloorToInt(i.maxHp*0.02f);
						i.Damage(hpReduction);
					}
				}

			}

		}

		public override bool OverrideAttack(EntityBase target,int attack) {
			if(attack==-1) {

				if(timeRemaining<=0) {
					timeRemaining=duration;
					maxMana=Player.instance.mana;
				} else {
					timeRemaining=-1;
				}

				return true;
			} else if(attack==0) {
				if(timeRemaining>0) entity.DoAttack(2);
				else entity.DoAttack(4);
				return true;
			} else if(attack==1) {
				if(timeRemaining>0) entity.DoAttack(3);
				else entity.DoAttack(5);
				return true;
			}

			return false;
		}


	}

}
