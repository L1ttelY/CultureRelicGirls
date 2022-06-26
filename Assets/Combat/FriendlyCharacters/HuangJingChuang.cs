using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class HuangJingChuang : EntityFriendly
	{

		public float buffDamage = 10.0f;
		public float buffTime;
		public GameObject attPre;
		float addDamage;
		float times;
		int AttackTimes = 0;
		int isAnima = 0;
		protected override ProjectileBase Attack(EntityBase target) //������ʱ��
		{
		
			AttackTimes += 1;
			if (AttackTimes % 2 == 0) //ÿ���¹���ˢ��һ��buff
			{
				times = buffTime;
				addDamage = buffDamage;
				isAnima = 3;
			}

			return base.Attack(target);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			times -= Time.deltaTime;
			if(times <=0)
            {
				addDamage = 0;
            }
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
			if (! (sender is EntityFriendly)) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=

			sender.powerBuff += addDamage/sender.attackBasePower; //���ӹ�����
			if (isAnima-- >0)
			{
				GameObject a = Instantiate(attPre, sender.transform.position, Quaternion.identity);
				a.transform.parent = sender.transform;
			}
		}
	}

}