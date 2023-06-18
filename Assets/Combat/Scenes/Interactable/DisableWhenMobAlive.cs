using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class DisableWhenMobAlive:MonoBehaviour {

		[SerializeField] GameObject[] targetMobs;
		[SerializeField] MonoBehaviour target;

		bool activated;

		private void FixedUpdate() {

			if(!activated) {
				activated=true;
				foreach(var i in targetMobs) {
					if(i) { activated=false; break; }
				}
			}

			target.enabled=activated;

			if(target is Interactable) {
				Interactable i = target as Interactable;
				i.status=activated ? Interactable.InteractableStatus.Enabled : Interactable.InteractableStatus.Supressed;
			}

		}

	}

}