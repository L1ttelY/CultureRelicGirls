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
			Debug.Log(this.tag);
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
            EntityBase.DamageEvent += EntityBase_DamageEvent;
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			times -= Time.deltaTime;
			if (isAnima)
				animaTime -= Time.deltaTime;
        }

        private void EntityBase_DamageEvent(object sender, DamageModel e)
        {
			enermy = sender ;
			times = SkillTime;
			isAnima = true;
		}

        protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
			EntityBase.DamageEvent -= EntityBase_DamageEvent;
		}

		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender!=this) return;
			if (times > 0)
				(enermy as EntityBase).speedBuff -= SpeedDebuff;
			if(isAnima && animaTime <=0)
            {
				animaTime = SkillTime;
				GameObject a = Instantiate(slowPre, (enermy as EntityBase).transform.position, Quaternion.identity);
                a.transform.parent = (enermy as EntityBase).transform;//减速特效
				
            }
        }
	}

}