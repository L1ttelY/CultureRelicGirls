using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class SiMuWuDing : EntityFriendly
	{
		public int antiDamage;
		public GameObject antiPre;
		public float animaTime = 0.21f;
		float times = 0;
		public override void Damage(DamageModel e) //Damage����������ʱ�������˺���
        {
            base.Damage(e);
			//���÷����˺�ģ��
			DamageModel returnDamage = GetDamage();
			returnDamage.amount = antiDamage;
			returnDamage.knockback = GetDamage().knockback/3;
			//�Ե����ճ��˺�
			e.dealer.Damage(returnDamage);

			if (times >= animaTime)
			{
				VfxPool.Create(antiPre, this.transform.position, Direction.right);
				times = 0;
			}

		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			times += Time.deltaTime;
        }

        protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
		}
		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
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