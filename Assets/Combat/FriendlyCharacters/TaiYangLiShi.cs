using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class TaiYangLiShi:EntityFriendly {

		public float SpeedDebuff = 1f;
		public float SkillTime = 1.1f;
		public GameObject slowPre;
		Dictionary<EntityEnemy,float> enermy = new Dictionary<EntityEnemy,float>();

		protected override void Start() {
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
			EntityBase.DamageEvent+=EntityBase_DamageEvent;
		}

		static KeyValuePair<EntityEnemy,float>[] buffer = new KeyValuePair<EntityEnemy,float>[1000];
		protected override void FixedUpdate() {
			base.FixedUpdate();

			int c = 0;
			foreach(var i in enermy) {
				buffer[c]=i;
				c++;
			}
			for(int i = 0;i<c;i++) {
				enermy[buffer[i].Key]=buffer[i].Value-Time.deltaTime;
			}

		}

		private void EntityBase_DamageEvent(object _sender,DamageModel e) {

			if(e.dealer!=this) return;

			EntityEnemy sender = _sender as EntityEnemy;
			if(!enermy.ContainsKey(sender)) {
				enermy.Add(sender,SkillTime);
			} else {
				enermy[sender]=SkillTime;
			}

			GameObject a = Instantiate(slowPre,sender.transform.position,Quaternion.identity);
			a.transform.parent=sender.transform;//减速特效

		}

		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
			base.OnDestroy();
		}

		private void EntityBase_UpdateStats(object _sender) {
			//判断是否响应
			EntityEnemy sender = _sender as EntityEnemy;
			if(sender==null) return;
			if(!enermy.ContainsKey(sender)) return;
			if(enermy[sender]>0) sender.speedBuff-=SpeedDebuff;

		}
	}

}