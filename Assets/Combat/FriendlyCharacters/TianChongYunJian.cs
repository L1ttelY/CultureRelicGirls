using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TianChongYunJian : EntityFriendly
	{

		public float SkillDamage = 0.05f;
		int AttackTimes = 0;

		protected override ProjectileBase Attack(EntityBase target) //当攻击时：
		{
			AttackTimes += 1;
			return base.Attack(target);
			if (AttackTimes % 4 == 0) ;//技能在这里
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