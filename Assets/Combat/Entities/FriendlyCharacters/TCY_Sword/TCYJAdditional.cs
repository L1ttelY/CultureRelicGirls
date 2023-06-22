using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class TCYJAdditional:EntityAdditionalFunctionBase {

		[SerializeField] GameObject projectilePrefab;

		public override bool OverrideAttack(EntityBase target,int attack) {

			if(attack==-1) {

				foreach(var i in EntityBase.entities) {
					EntityEnemy e = i as EntityEnemy;
					if(e==null) continue;
					if(!e.isActiveAndEnabled) continue;
					if(Mathf.Abs(e.transform.position.x-transform.position.x)>10) continue;
					if(e.isStaggered) {
						DamageModel damage = new DamageModel();
						damage.dealer=entity;
						damage.amount=entity.attackBasePower;
						damage.damageType=DamageType.Slash;
						damage.direction=(e.transform.position.x-transform.position.x)>0 ? Direction.right : Direction.left;
						ProjectilePool.Create(projectilePrefab,e.transform.position,Vector2.zero,e,entity.isFriendly,damage);
					}
				}

				return true;
			}

			return false;
		}

	}

}
