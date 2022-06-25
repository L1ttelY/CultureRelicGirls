using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class SiMuWuDing : EntityFriendly
	{
		public int antiDamage;
      
		public override void Damage(DamageModel e) //Damage����������ʱ�������˺���
        {
            base.Damage(e);
			//���÷����˺�ģ��
			DamageModel returnDamage = GetDamage();
			returnDamage.amount = antiDamage;
			returnDamage.knockback = GetDamage().knockback/3;
			//�Ե����ճ��˺�
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
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=

		}
	}

}