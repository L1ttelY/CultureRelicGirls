using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class CreateVfxOnHorizon:MonoBehaviour {

		[SerializeField] AudioClip soundActivation;
		[SerializeField] GameObject vfxPrefab;
		bool used;

		private void OnEnable() {
			used=false;
		}

		private void Update() {

			if(!used&&transform.position.y<=-0.5f) {
				used=true;
				Vector3 position = transform.position;
				position.y=-0.5f;
				AudioController.PlayAudio(soundActivation,transform.position);
				VfxPool.Create(vfxPrefab,position,Direction.right);
			}

		}

	}

}