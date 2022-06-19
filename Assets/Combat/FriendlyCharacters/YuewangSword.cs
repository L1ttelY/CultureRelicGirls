using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	/*
	 *Ѫ������50% ����+75%
	 *Ѫ������25% ����+100%
	 *Ѫ������
	 */

	public class YuewangSword:EntityFriendly {

		float desiredPowerBuff;

		Color stage1 = new Color(1,.7f,.7f);
		Color stage2 = new Color(1,0,0);

		protected override void FixedUpdate() {
			base.FixedUpdate();

			//�ж�����������
			desiredPowerBuff=0;
			float hpPerentage = (float)hp/(float)maxHp;
			if(hpPerentage>0.5f) spriteRenderer.color=Color.white;
			if(hpPerentage<=0.5f) { desiredPowerBuff+=0.75f; spriteRenderer.color=stage1; }
			if(hpPerentage<=0.25f) { desiredPowerBuff+=0.25f; spriteRenderer.color=stage2; }

		}

		protected override void Start() {
			base.Start();
			EntityBase.UpdateStats+=EntityBase_UpdateStats;
		}
		protected override void OnDestroy() {
			base.OnDestroy();
			EntityBase.UpdateStats-=EntityBase_UpdateStats;
		}


		private void EntityBase_UpdateStats(object _sender) {
			//�ж��Ƿ���Ӧ
			EntityBase sender = _sender as EntityBase;
			if(sender!=this) return;

			//׼����Ӧ
			//ע����+= ��Ҫ��*=��=
			powerBuff+=desiredPowerBuff;

		}


	}

}