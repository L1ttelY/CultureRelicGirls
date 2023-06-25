using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class QMXAdditional:EntityAdditionalFunctionBase {
		[SerializeField] float rangeMin;
		[SerializeField] float rangeMax;
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
					entity.Attack(null,targetAttackIndex);
				} else {
					int targetIndex = Random.Range(0,targets.Count);
					entity.Attack(targets[targetIndex],targetAttackIndex);
				}

				return true;
			}

			return false;

		}

	}

}
