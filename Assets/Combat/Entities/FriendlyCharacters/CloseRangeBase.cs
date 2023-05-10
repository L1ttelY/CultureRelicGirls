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
			//��ײ�˺�
			int cnt = collider.Cast(velocity,Utility.raycastBuffer,Mathf.Abs(velocity.x)*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityBase other = hit.collider.GetComponent<EntityBase>();
				if(other&&(!other.isFriendly)) OnChargeHit(other,!other.isKnockbacked);
			}
		}

		protected virtual void OnChargeHit(EntityBase target,bool actualHit) {
			DamageModel damage = GetDamage();

			damage.amount=Mathf.RoundToInt(damage.amount*1.1f);
			damage.direction=velocity.x>0 ? Direction.right : Direction.left;
			damage.damageType=DamageType.Contact;
			damage.knockback=10;
			if(!actualHit) damage.amount=0;
			target.Damage(damage);
		}

	}

}