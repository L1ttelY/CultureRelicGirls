using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class SiMuWuDing : EntityFriendly
	{
		public int antiDamge = 10;
		
		protected override void FixedUpdate()
        {
			base.FixedUpdate();
			
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
			DamageModel returnDamage;
			returnDamage.amount = antiDamge;
			returnDamage.knockback = 0;
			returnDamage.dealer = result.dealer;
			returnDamage.direction = result.direction;
			
			if (isBeenAttacked)
            {
				WhoIsAttackMe.Damage(returnDamage);
			}

		}
		protected override void Start()
		{
			base.Start();
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=

		}
	}

}