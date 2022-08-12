using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class CloseRangeBase:EntityFriendly {

		public override void Damage(DamageModel e) {
			if(isCharging) return;
			base.Damage(e);
		}

		protected override void Update() {
			base.Update();
			if(isCharging) UpdateContactDamage();
		}

		protected virtual void UpdateContactDamage() {
			//Åö×²ÉËº¦
			int cnt = collider.Cast(velocity,Utility.raycastBuffer,Mathf.Abs(velocity.x)*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityEnemy other = hit.collider.GetComponent<EntityEnemy>();
				if(other) OnChargeHit(other);
			}
		}

		protected virtual void OnChargeHit(EntityEnemy target) {
			DamageModel damage = GetDamage();

			if(target.currensState==StateKnockback) damage.amount=0;

			damage.amount=Mathf.RoundToInt(damage.amount*1.1f);
			damage.direction=velocity.x>0 ? Direction.right : Direction.left;
			damage.damageType=DamageType.Contact;
			target.Damage(damage);
		}

	}

}