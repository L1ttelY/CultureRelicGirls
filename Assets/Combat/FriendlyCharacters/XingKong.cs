using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class XingKong : EntityFriendly
	{
		[field: SerializeField] public GameObject stars { get; protected set; }//���л��ֶΣ����䱩¶
		public int SkillDamage = 50;
		public float skillTime = 1.2f;
		public float Height = 20;
		int AttackTimes = 0;
		float coolTime = 3;

		Vector2 vec;
		DamageModel starDamage;
		EntityBase[] enermy = new EntityBase[4];
		
		void sort()
        {
			for (int j = 0; j < 2 ; j++)
			{
				if(enermy[j].transform.position.x > enermy[j + 1].transform.position.x)
                {
					enermy[3] = enermy[j + 1];
					enermy[j + 1] = enermy[j];
					enermy[j] = enermy[3];
                }
			}
		}

		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
			EntityBase.DamageEvent += EntityBase_DamageEvent; 
			vec.x = 0;
			vec.y = -1;
			starDamage = GetDamage();
			starDamage.amount = SkillDamage;
			starDamage.knockback = GetDamage().knockback / 3;

		}

        private void EntityBase_DamageEvent(object sender, DamageModel e)
        {
			if (coolTime > skillTime)
			{
				AttackTimes += 1;
				if (AttackTimes % 3 == 0) //����������
				{
					Vector2 position;
					foreach (var i in entities) //�����ʼ��
					{
						if (!(i is EntityFriendly))
						{
							enermy[0] = i;
							enermy[1] = i;
							enermy[2] = i;
							break;
						}
					}

					sort();

					foreach (var i in entities) //��������
					{
						//�Ե��˽�������
						if (!(i is EntityFriendly))
						{
							if (i.transform.position.x < enermy[2].transform.position.x)
							{
								enermy[2] = i;
								sort();
							}
						}
					}
					//�������еĵ����������ճ��˺�
					position.x = enermy[2].transform.position.x;
					position.y = enermy[2].transform.position.y + Height;
					if (position.x - this.transform.position.x < 15)
					{
						ProjectilePool.Create(stars, position, vec, enermy[2], true, starDamage);
						coolTime = 0;
						timeAfterAttack = 2 * attackCd - skillTime; //�����������
					}
				} 
			}
			
		}

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
			coolTime += Time.deltaTime;
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

		}
	}

}