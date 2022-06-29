using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class SiYangFangZhun:EntityFriendly {

		[SerializeField] AudioClip soundSkill;

		public float HealBuff = 0.05f;
		public GameObject healPre;
		int AttackTimes = 0;
		protected override ProjectileBase Attack(EntityBase target) //当攻击时：
		{

			AttackTimes+=1;

			if(AttackTimes%4==0) { //打第四下时发动技能

				AudioController.PlayAudio(soundSkill,transform.position);

				foreach(var i in entities) //遍历对象寻找队友
								{
					if(i is EntityFriendly) {
						i.Heal((int)(HealBuff*(float)(i.maxHp-i.hp))); //恢复血量
						GameObject a = Instantiate(healPre,i.transform.position,Quaternion.identity);
						a.transform.parent=i.transform;

					}
				}
			}
			return base.Attack(target);
		}


		protected override void Start() {
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
		}
		protected override void OnDestroy() {
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
		}
		private void EntityBase_UpdateStats(object _sender) {

			//判断是否响应
			EntityBase sender = _sender as EntityBase;
			if(sender!=this) return;

			//准备相应
			//注意用+= 不要用*=或=

		}
	}

}