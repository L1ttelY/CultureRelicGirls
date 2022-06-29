using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	/*
	 *血量低于50% 攻击+75%
	 *血量低于25% 攻击+100%
	 *血量低于
	 */

	public class YuewangSword:EntityFriendly {

		float desiredPowerBuff;
		
		protected override void FixedUpdate() {

			base.FixedUpdate();
			//判断增幅量备用
			desiredPowerBuff=0;
			float hpPerentage = (float)hp/(float)maxHp;
			if (hpPerentage > 0.5f)
			{
				this.GetComponentInChildren<YueWangSkill>().skillStates = 0;
			}
			if(hpPerentage<=0.5f) { 
				desiredPowerBuff+=0.75f; 
				this.GetComponentInChildren<YueWangSkill>().skillStates = 1;
			}

			if (hpPerentage<=0.25f) { 
				desiredPowerBuff+=0.25f; 
				this.GetComponentInChildren<YueWangSkill>().skillStates = 2;
			}
		}

		protected override void Start() {
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
			
		}
		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
		}


		private void EntityBase_UpdateStats(object _sender) {
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if(sender!=this) return;

			//准备相应
			//注意用+= 不要用*=或=
			powerBuff+=desiredPowerBuff;
		}


	}

}