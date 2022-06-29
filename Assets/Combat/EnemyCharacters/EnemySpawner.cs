using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class EnemySpawner:EntityEnemy {

		[SerializeField] GameObject[] spawnedPrefab = new GameObject[4];
		[SerializeField] GameObject deathVfx;
		protected override ProjectileBase Attack(EntityBase target) {

			AudioController.PlayAudio(soundAttack,transform.position);

			animator.SetTrigger("attack");
			
			int enermyNumber = 0;
			switch (Random.Range(0, 9))
            {
				case 0 : enermyNumber = 0;break; //ÌøÌø
				case 1 :
				case 2 : enermyNumber = 1;break;//fighter
				case 3 :
				case 4 :
				case 5 : enermyNumber = 2;break;//block
				case 6 :
				case 7 :
				case 8 : enermyNumber = 3;break;//tanker
			}

			GameObject newGo = Instantiate(spawnedPrefab[enermyNumber],transform.parent);
			newGo.transform.position=transform.position;
			timeAfterAttack=0;

			return null;
		}

		protected override void StartKnockback(float knockback,int direction) { }
		protected override void OnDeath() {
			VfxPool.Create(deathVfx,transform.position,direction);
			base.OnDeath();
			Destroy(gameObject);
		}

	}
}