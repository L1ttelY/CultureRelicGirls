using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class TaiYangLiShi:EntityFriendly {

		public float SpeedDebuff = 0.5f;
		public float SkillTime = 1.1f;
		public GameObject slowPre;
		Dictionary<EntityEnemy,float> enermy;
		bool isAnima;
		float animaTime = 0;
		protected override void Start() {
			Debug.Log(this.tag);
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
			EntityBase.DamageEvent+=EntityBase_DamageEvent;
		}

		protected override void FixedUpdate() {
			base.FixedUpdate();

			foreach(var i in enermy) { enermy[i.Key]-=Time.deltaTime; }

			if(isAnima)
				animaTime-=Time.deltaTime;
		}

		private void EntityBase_DamageEvent(object sender,DamageModel e) {

			if(e.dealer!=this) return;

			if(enermy.ContainsKey(sender as EntityEnemy)) {
				enermy.Add(sender as EntityEnemy,SkillTime);
			} else {
				enermy[sender as EntityEnemy]=SkillTime;
			}

			isAnima=true;
		}

		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
			base.OnDestroy();
		}

		private void EntityBase_UpdateStats(object _sender) {
			//判断是否响应
			EntityEnemy sender = _sender as EntityEnemy;
			if(!enermy.ContainsKey(sender)) return;
			if(enermy[sender]>0) sender.speedBuff-=SpeedDebuff;
			if(isAnima&&animaTime<=0) {
				animaTime=SkillTime;
				GameObject a = Instantiate(slowPre,sender.transform.position,Quaternion.identity);
				a.transform.parent=sender.transform;//减速特效

			}
		}
	}

}