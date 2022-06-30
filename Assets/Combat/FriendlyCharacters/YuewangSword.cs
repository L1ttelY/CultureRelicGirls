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

		[SerializeField] AudioClip soundSkill;

		float desiredPowerBuff;

		float previousPowerBuff;

		protected override void FixedUpdate() {

			base.FixedUpdate();
			//�ж�����������
			desiredPowerBuff=0;
			float hpPerentage = (float)hp/(float)maxHp;
			if(hpPerentage>0.5f) {
				this.GetComponentInChildren<YueWangSkill>().skillStates=0;
			}
			if(hpPerentage<=0.5f) {
				desiredPowerBuff+=0.25f;
				this.GetComponentInChildren<YueWangSkill>().skillStates=1;
			}

			if(hpPerentage<=0.25f) {
				desiredPowerBuff+=0.25f;
				this.GetComponentInChildren<YueWangSkill>().skillStates=2;
			}

			if(desiredPowerBuff>previousPowerBuff) AudioController.PlayAudio(soundSkill,transform.position);
			previousPowerBuff=desiredPowerBuff;

		}

		protected override void Start() {
			base.Start();
			UpdateStats+=EntityBase_UpdateStats;

		}
		protected override void OnDestroy() {
			UpdateStats-=EntityBase_UpdateStats;
			base.OnDestroy();
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