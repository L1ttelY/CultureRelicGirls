using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Fighter:EntityEnemy {
		[SerializeField] float knockbackResistance = 1;
		float CurrentKnockbackResistance = 0; //此时刻的击退抗性
		public Sprite BlockDefine; //得到block的精灵，用于鉴别block
		public Sprite TankDefine; //得到Tank的精灵，用于鉴别tank


		protected override void DoKnockback(float knockback,int direction) {
			base.DoKnockback(knockback*(1-CurrentKnockbackResistance),direction);//这里的变量换了下
		}
		protected override void Update() {
			base.Update();

			CurrentKnockbackResistance=knockbackResistance;

			foreach(var i in entities) {
				//判断前方有没有盾
				if(i is EntityEnemy) {
					float distance = this.transform.position.x-i.transform.position.x;
					Sprite whoAmI = i.GetComponentInChildren<SpriteRenderer>().sprite;
					if(whoAmI==BlockDefine&&distance<5) //前方是盾，距离小
					{
						CurrentKnockbackResistance=0.9f;
						break;
					}
					if(whoAmI==TankDefine&&distance<5) {
						break;
					}
				}
				CurrentKnockbackResistance=knockbackResistance;
			}
		}

	}

}