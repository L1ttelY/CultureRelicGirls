using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Combat {

	public class QMXAdditional:EntityAdditionalFunctionBase {
		[SerializeField] float rangeMin;
		[SerializeField] float rangeMax;
		[SerializeField] float slowAmount;
		[SerializeField] float slowTime;
		[SerializeField] float damageMultiplier = 1;
		[SerializeField] int targetAttackIndex = 1;

		static List<EntityBase> targets = new List<EntityBase>();

		public override bool OverrideAttack(EntityBase target,int attack) {

			if(attack==-1) {

				targets.Clear();
				foreach(var i in EntityBase.entities) {
					if(i.isFriendly) continue;
					if(!i.isActiveAndEnabled) continue;
					if(Mathf.Abs(i.transform.position.x-transform.position.x)<rangeMin) continue;
					if(Mathf.Abs(i.transform.position.x-transform.position.x)>rangeMax) continue;
					targets.Add(i);
				}
				if(targets.Count==0) {
					entity.powerBuff*=damageMultiplier;
					entity.Attack(null,targetAttackIndex);
				} else {
					int targetIndex = Random.Range(0,targets.Count);
					float powerBuff = entity.powerBuff;
					entity.powerBuff*=damageMultiplier;
					entity.Attack(targets[targetIndex],targetAttackIndex);
					entity.powerBuff=powerBuff;
				}

				return true;
			}

			return false;

		}
		protected override void Start() {
			base.Start();
			EntityBase.DamageEvent+=EntityBase_DamageEvent;
		}

		private void EntityBase_DamageEvent(object _sender,DamageModel e) {
			if(slowAmount==0||slowTime==0) return;
			if(e.dealer!=entity) return;
			EntityBase sender = _sender as EntityBase;
			var targetBuff = sender.buffSlot[typeof(BuffSlow)] as BuffSlow;
			targetBuff.amount=Mathf.Max(targetBuff.amount,slowAmount);
			targetBuff.time=Mathf.Max(targetBuff.time,slowTime);
		}

		private void OnDestroy() {
			EntityBase.DamageEvent-=EntityBase_DamageEvent;
		}

	}

	public class BuffSlow:BuffBase {

		public float amount;
		public float time;

		public BuffSlow() {
		}

		public override void Update() {
			base.Update();
			if(time>0)owner.speedBuff-=amount;
			time-=Time.deltaTime;
		}
	}

}
