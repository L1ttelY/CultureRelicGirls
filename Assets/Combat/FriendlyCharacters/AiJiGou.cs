using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class AiJiGou : EntityFriendly
	{
		
		DamageModel aoe;
		float times = 2;

		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
			EntityBase.DamageEvent += EntityBase_DamageEvent;
			aoe = GetDamage();
			aoe.amount = (int)(attackBasePower*powerBuff);
			aoe.knockback = 0;
			powerBuff = 0.1f;
		}

		private void EntityBase_DamageEvent(object sender, DamageModel e)
		{
			if (times > attackCd)
			{
				times = 0;
				foreach (var i in entities)
				{
					if (!(i is EntityFriendly))
					{
						if (i.transform.position.x - this.transform.position.x < 10 )
						{
							i.Damage(aoe);
						}
					}
				}
			}
	    }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			times += Time.deltaTime;
        }

        protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
			EntityBase.DamageEvent -= EntityBase_DamageEvent;

		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=
			Debug.Log(this.tag);
		}
	}

}