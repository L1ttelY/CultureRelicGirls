using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class XingKong : EntityFriendly
	{
		[field: SerializeField] public GameObject stars { get; protected set; }//���л��ֶΣ����䱩¶
		public int SkillDamage = 50;
		int AttackTimes = 0;
		Vector2 vec;
		DamageModel starDamage;
		EntityBase[] enermy = new EntityBase[4];
		
		void sort()
        {
			for (int j = 0; j <2 ; j++)
			{
				if(enermy[j].transform.position.x > enermy[j + 1].transform.position.x)
                {
					enermy[3] = enermy[j + 1];
					enermy[j + 1] = enermy[j];
					enermy[j] = enermy[3];
                }
			}
		}

		protected override ProjectileBase Attack(EntityBase target) //������ʱ��
		{

			AttackTimes += 1;
			if (AttackTimes % 3 == 0) //����������
			{
				Vector2 position;
				foreach(var i in entities) //�����ʼ��
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
				//�������е����������ճ��˺�
				for(int z = 0; z < 3; z++)
                {
					position.x = enermy[z].transform.position.x;
					position.y = enermy[z].transform.position.y + 4f;
					ProjectilePool.Create(stars, position, vec, enermy[z], true, starDamage);
				}
			}
			Debug.Log(1111);
			return base.Attack(target);
		}

			


		protected override void Start()
		{
			base.Start();
			EntityBase.UpdateStats += EntityBase_UpdateStats;
			vec.x = 0;
			vec.y = -1;
			starDamage = GetDamage();
			starDamage.amount = SkillDamage;
			starDamage.knockback = GetDamage().knockback / 3;


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