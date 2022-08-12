using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class SiMuWuDing:CloseRangeBase {

		[SerializeField] Image energyBar;
		[SerializeField] SpriteRenderer shield;

		float maxEnergy;
		bool energyEarnedThisCharge;

		int shieldAmount;
		int shieldMax {
			get { return Mathf.RoundToInt(maxEnergy*shieldEfficiency); }
		}
		float shieldEfficiency {
			get { return 1.05f+use.level*0.05f; }
		}

		float timeAfterPassive = 999;
		[SerializeField] float passiveCooldown = 20;

		float energy;

		protected override void ChargeStart() {
			base.ChargeStart();
			energyEarnedThisCharge=false;
		}

		protected override void OnChargeHit(EntityEnemy target) {
			base.OnChargeHit(target);
			if(!energyEarnedThisCharge) {
				energyEarnedThisCharge=true;
				energy+=maxEnergy*((use.level>=8) ? 0.5f : 0.3f);
			}
		}

		public override void Damage(DamageModel e) {

			if(shieldAmount>0) {
				if(shieldAmount>=e.amount) {
					shieldAmount-=e.amount;
					return;
				} else {
					e.amount-=shieldAmount;
					shieldAmount=0;
				}
			}

			if(isCharging) return;

			//丰收
			energy+=e.amount;

			//青铜合金
			if(use.level>=4) {
				int negation = use.level*5;
				if(isSkill2Active) negation=Mathf.RoundToInt(negation*(0.2f*use.level-1));//主动技能2
				e.amount-=negation;
			}
			if(e.amount<0) e.amount=0;

			base.Damage(e);
		}

		void HealEffect(float amount) {

			Heal(Mathf.RoundToInt(amount));
			foreach(var i in friendlyList) {
				if(i==this) continue;
				i.Heal(Mathf.RoundToInt(0.5f*amount));
			}
		}

		protected override void Update() {
			base.Update();

			maxEnergy=maxHp*0.3f;
			energyBar.fillAmount=energy/maxEnergy;

			timeAfterPassive+=Time.deltaTime;

			shield.color=new Color(1,1,1,shieldAmount/shieldMax);

			if(energy>=maxEnergy) {
				energy=maxEnergy;

				//被动技能 消耗能量恢复血量
				if(use.actionSkillType!=1&&timeAfterPassive>=passiveCooldown) {
					timeAfterPassive=0;
					energy=0;
					HealEffect(0.75f*maxEnergy);
				}
			}

			//主动技能1 护盾结束时恢复血量
			if(timeAfterSkill>5&&shieldAmount!=0) {
				HealEffect(0.3f*shieldAmount);
				shieldAmount=0;
			}

			//主动技能2
			if(isSkill2Active) timeAfterPassive=999;

		}


		protected override void ActionSkill1() {
			if(timeAfterSkill<skillCd) return;
			timeAfterSkill=0;
			shieldAmount=Mathf.RoundToInt(energy*shieldEfficiency);
			energy=0;
		}

		float skill2Duration = 20f;

		bool isSkill2Active { get { return use.actionSkillType==2&&timeAfterSkill<skill2Duration; } }
		protected override void ActionSkill2() {
			if(timeAfterSkill<skillCd) return;
			timeAfterSkill=0;
		}


	}

}