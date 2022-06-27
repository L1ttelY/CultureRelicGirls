using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class EnemySpawner:EntityEnemy {

		[SerializeField] GameObject spawnedPrefab;
		[SerializeField] GameObject deathVfx;
		protected override ProjectileBase Attack(EntityBase target) {

			animator.SetTrigger("attack");
			GameObject newGo = Instantiate(spawnedPrefab,transform.parent);
			newGo.transform.position=transform.position;
			timeAfterAttack=0;

			return null;
		}

		protected override void StartKnockback(float knockback,int direction) { }
		protected override void OnDeath() {
			VfxPool.Create(deathVfx,transform.position,direction);
			base.OnDeath();
		}

	}
}