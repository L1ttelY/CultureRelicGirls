using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class CreateVfxOnHorizon:MonoBehaviour {

		[SerializeField] GameObject vfxPrefab;
		bool used;

		private void OnEnable() {
			used=false;
		}

		private void Update() {

			if(!used&&transform.position.y<=0) {
				used=true;
				VfxPool.Create(vfxPrefab,transform.position,Direction.right);
			}

		}

	}

}