using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	/*
	 *血量低于50% 攻击+75%
	 *血量低于25% 攻击+100%
	 *血量低于
	 */

	public class SiMuWuDing : EntityFriendly
	{
		protected override void FixedUpdate()
        {
			base.FixedUpdate();


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