using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class ShuangTouShe : EntityFriendly
	{
		public float BuffTime = 1.9f;
		public float AddAttackPerBuff = 5.0f;
		int AttackTimes = 0;
		float time = 0;
		float desireAttackBuff;
		float[] animaTime;
        protected override ProjectileBase Attack(EntityBase target)
        {
			AttackTimes = Mathf.Clamp(AttackTimes + 1, 0, 5);
			time = BuffTime; 
			return base.Attack(target);

		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if(time>0)
             {
				time -= Time.deltaTime; //��ʱ
                
				//����ʱ��������buff������buffʱ��
				if (time <= 0.0f && AttackTimes!=0)  
				{ 
					AttackTimes -= 1;
					time = BuffTime;
				}
				//����ʱû��������buff
				desireAttackBuff = ((float)AttackTimes * AddAttackPerBuff) / attackBasePower;
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
			if (sender != this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=
			powerBuff += desireAttackBuff;
			GetComponentInChildren<ShuangTouSheSkill>().buff = AttackTimes;

		}
	}

}