using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityAdditionalFunctionBase:MonoBehaviour {
		protected EntityBase entity;
		protected virtual void Start() {
			entity=GetComponent<EntityBase>();
		}

		public virtual bool OverrideAttack(EntityBase target,int attack) { return false; }
	}

}
