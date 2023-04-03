using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Interactable:MonoBehaviour {

		static HashSet<Interactable> instances = new HashSet<Interactable>();
		public static bool CanInteract() {
			foreach(var i in instances) {
				if(!i.enabled) continue;
				if(i.distanceToCharacter<0.75f) return true;
			}
			return false;
		}
		public static void DoInteract() {
			if(!CanInteract()) return;
			float minDistance = 1;
			Interactable target = null;
			foreach(var i in instances) {
				if(!i.enabled) continue;
				if(i.distanceToCharacter<minDistance) {
					minDistance=i.distanceToCharacter;
					target=i;
				}
			}

			if(target==null) return;
			target.onInteract.Invoke();

		}

		[SerializeField] UnityEngine.Events.UnityEvent onInteract;
		private void Update() {
			distanceToCharacter=1;
			for(int i = 0;i<3;i++) {
				if(!EntityFriendly.friendlyList[i]) continue;
				float newDistance = Mathf.Abs(EntityFriendly.friendlyList[i].transform.position.x-transform.position.x);
				if(newDistance<distanceToCharacter) distanceToCharacter=newDistance;
			}
		}

		float distanceToCharacter;

		private void Start() {
			instances.Add(this);
		}
		private void OnDestroy() {
			instances.Remove(this);
		}
		private void OnDisable() {
			distanceToCharacter=1;
		}

	}

}
