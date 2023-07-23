using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class GuillotineAdditional:EntityAdditionalFunctionBase {

		[Tooltip("���ܳ���ʱ��")]
		[SerializeField] float duration;
		[Tooltip("ÿ������ʧѪ��ռ��Ѫ���ı���")]
		[SerializeField] float hpLossRate;
		[Tooltip("��Ѫ����")]
		[SerializeField] float hpRecoveryRate;
		[Tooltip("�����ظ�����")]
		[SerializeField] float manaRegenRate;
		[SerializeField] Color skillColor;

		float timeRemaining;

		float maxMana;
		float hpLossBuildup;

		SpriteRenderer spriteRenderer;

		protected override void Start() {
			base.Start();
			EntityBase.DamageEvent+=EntityBase_DamageEvent;
			spriteRenderer=entity.spriteRenderer;
			if(skillColor==Color.clear) skillColor=Color.red;
		}

		private void EntityBase_DamageEvent(object sender,DamageModel e) {
			if(timeRemaining<=0) return;
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

			spriteRenderer.color=timeRemaining>0 ? skillColor : Color.white;

			//���ܼ���
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
			}

			return false;
		}


	}

}
