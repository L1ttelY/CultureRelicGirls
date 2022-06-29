using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TaiYangLiShi : EntityFriendly
	{
      
		public float SpeedDebuff = 0.5f;
		public float SkillTime = 1.1f;
		public GameObject slowPre;
		object enermy;
		float times=0;
		bool isAnima;
		float animaTime = 0;
		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
            EntityBase.DamageEvent += EntityBase_DamageEvent;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			times -= Time.deltaTime; //计时器
			
			if (isAnima) //放动画的过程中，animaTime减少，记录动画放了多久
				animaTime -= Time.deltaTime;
        }

        private void EntityBase_DamageEvent(object sender, DamageModel e)
        {
			enermy = sender ; //记录被我打的人
			times = SkillTime; //记录减速的时间
			isAnima = true; //记录开始放动画
		}

        protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
			base.OnDestroy();
		}

		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender!=this) return;
			if (times > 0)
				(enermy as EntityBase).speedBuff -= SpeedDebuff;
			
			if(isAnima && animaTime <=0) //正在放动画并且animaTime减到0下了
            {
				animaTime = SkillTime; //重置animaTime，是为了防止重复刷新特效。
				//播放减速特效
				GameObject a = Instantiate(slowPre, (enermy as EntityBase).transform.position, Quaternion.identity);
                a.transform.parent = (enermy as EntityBase).transform;
            }
        }
	}


}