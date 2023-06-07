using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat{

  public class GuillotineAdditional : EntityAdditionalFunctionBase{

    [SerializeReference] GameObject objectToSummon;

		public override bool OverrideAttack(EntityBase target,int attack) {

			if(attack==-1){
				Instantiate(objectToSummon,transform.position,Quaternion.identity,transform.parent);
				return true;
			}

			return false;
		}

	}

}
