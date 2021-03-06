using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

	public class HuangJingChuang : EntityFriendly
	{

		[SerializeField] AudioClip skillSound;

		public float buffDamage = 10.0f;
		public float buffTime;
		public GameObject attPre;
		float addDamage;
		float times;
		int AttackTimes = 0;
		int isAnima = 0;
		protected override ProjectileBase Attack(EntityBase target) {//当攻击时：

			AttackTimes+=1;
			if(AttackTimes%2==0) {
				//每两下攻击刷新一次buff
				times=buffTime;
				addDamage=buffDamage;
				isAnima=3;
				AudioController.PlayAudio(skillSound,transform.position);
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
		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
		}
		private void EntityBase_UpdateStats(object _sender)
		{
			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if (! (sender is EntityFriendly)) return;

			//增加攻击力
			sender.powerBuff += addDamage/sender.attackBasePower; 
			
			//生成特效
			if (isAnima-- >0)
			{
				GameObject a = Instantiate(attPre, sender.transform.position, Quaternion.identity);
				a.transform.parent = sender.transform;
			}
		}
	}

}