using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class ShuangTouShe:EntityFriendly {
		[SerializeField] AudioClip soundSkill;

		public float BuffTime = 1.9f;
		public float AddAttackPerBuff = 5.0f;
		int AttackTimes = 0;
		float time = 0;
		float desireAttackBuff;
		float desiredCdRateBuff;
		float[] animaTime;
		protected override ProjectileBase Attack(EntityBase target) {
			if(AttackTimes==0||AttackTimes==5) time=BuffTime;
			AudioController.PlayAudio(soundSkill,transform.position);
			AttackTimes=Mathf.Clamp(AttackTimes+1,0,5);
			return base.Attack(target);

		}

		protected override void FixedUpdate() {
			base.FixedUpdate();
			if(time>0) {
				time-=Time.deltaTime; //计时

				//倒计时结束，掉buff，重置buff时间
				if(time<=0.0f&&AttackTimes!=0) {
					AttackTimes-=1;
					time=BuffTime;
				}

				//倒计时没结束，上buff
				desireAttackBuff=((float)AttackTimes*AddAttackPerBuff)/attackBasePower;
				desiredCdRateBuff=AttackTimes*0.1f;
			}
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
			powerBuff+=desireAttackBuff;
			cdSpeed+=desiredCdRateBuff;
			GetComponentInChildren<ShuangTouSheSkill>().buff=AttackTimes;

		}
	}

}