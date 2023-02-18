using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Fighter:EntityEnemy {
		[SerializeField] float knockbackResistance = 1;
		float CurrentKnockbackResistance = 0; //��ʱ�̵Ļ��˿���
		public Sprite BlockDefine; //�õ�block�ľ��飬���ڼ���block
		public Sprite TankDefine; //�õ�Tank�ľ��飬���ڼ���tank


		protected override void DoKnockback(float knockback,int direction) {
			base.DoKnockback(knockback*(1-CurrentKnockbackResistance),direction);//����ı���������
		}
		protected override void Update() {
			base.Update();

			CurrentKnockbackResistance=knockbackResistance;

			foreach(var i in entities) {
				//�ж�ǰ����û�ж�
				if(i is EntityEnemy) {
					float distance = this.transform.position.x-i.transform.position.x;
					Sprite whoAmI = i.GetComponentInChildren<SpriteRenderer>().sprite;
					if(whoAmI==BlockDefine&&distance<5) //ǰ���Ƕܣ�����С
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