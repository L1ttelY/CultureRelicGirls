using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class SiMuWuDing : EntityFriendly
	{
		public int antiDamage;
      
		public override void Damage(DamageModel e) //Damage函数被调用时，触发此函数
        {
            base.Damage(e);
			//设置返回伤害模型
			DamageModel returnDamage = GetDamage();
			returnDamage.amount = antiDamage;
			returnDamage.knockback = GetDamage().knockback/3;
			//对敌人照成伤害
			e.dealer.Damage(returnDamage);
        }
		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();
			EntityBase.UpdateStats -= EntityBase_UpdateStats;
		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//准备相应
			//注意用+= 不要用*=或=

		}
	}

}