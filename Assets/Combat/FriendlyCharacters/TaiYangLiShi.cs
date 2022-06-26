using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TaiYangLiShi : EntityFriendly
	{
      
		public float SpeedDebuff = 0.5f;
		public float SkillTime;
		object enermy;
		float times=0;
		
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
        }

        private void EntityBase_DamageEvent(object sender, DamageModel e)
        {
			enermy = sender ;
			times = SkillTime;
		}

        protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
			EntityBase.DamageEvent -= EntityBase_DamageEvent;
		}

		private void EntityBase_UpdateStats(object _sender)
		{
			//ÅÐ¶ÏÊÇ·ñÏìÓ¦
			EntityBase sender = _sender as EntityBase;
			if (sender!=this) return;
			if (times > 0)
				(enermy as EntityBase).speedBuff -= SpeedDebuff;
		}
	}

}