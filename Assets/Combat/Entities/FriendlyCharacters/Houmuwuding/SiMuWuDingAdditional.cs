using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class SiMuWuDingAdditional:EntityAdditionalFunctionBase {

		[SerializeField] float healAmount=0.3f;

		public override bool OverrideAttack(EntityBase target,int attack) {

			if(attack==-1) {

				foreach(var i in EntityFriendly.friendlyList) {
					if(i) i.Heal((int)(i.maxHp*healAmount));
				}

				return true;
			}

			return false;

		}

	}

}
