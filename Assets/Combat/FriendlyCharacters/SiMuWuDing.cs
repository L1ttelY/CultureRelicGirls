using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	/*
	 *Ѫ������50% ����+75%
	 *Ѫ������25% ����+100%
	 *Ѫ������
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
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=

		}
	}

}