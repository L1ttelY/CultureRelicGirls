using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class TianChongYunJian : EntityFriendly
	{

		public float SkillDamage = 0.05f;
		int AttackTimes = 0;

		protected override ProjectileBase Attack(EntityBase target) //������ʱ��
		{
			AttackTimes += 1;
			return base.Attack(target);
			if (AttackTimes % 4 == 0) ;//����������
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