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
				time -= Time.deltaTime; //计时
                
				//倒计时结束，掉buff，重置buff时间
				if (time <= 0.0f && AttackTimes!=0)  
				{ 
					AttackTimes -= 1;
					time = BuffTime;
				}
				//倒计时没结束，上buff
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
			
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (sender != this) return;

			//准备相应
			//注意用+= 不要用*=或=
			powerBuff += desireAttackBuff;
			GetComponentInChildren<ShuangTouSheSkill>().buff = AttackTimes;

		}
	}

}