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
		public static Interactable GetTarget() {
			if(!CanInteract()) return null;
			float minDistance = 1;
			Interactable target = null;
			foreach(var i in instances) {
				if(!i.enabled) continue;
				if(i.distanceToCharacter<minDistance) {
					minDistance=i.distanceToCharacter;
					target=i;
				}
			}

			if(target==null) return null;
			return target;
		}
		public static void DoInteract() {
			if(!CanInteract()) return;
			GetTarget()?.onInteract?.Invoke();
		}

		static Interactable currentTarget;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			EventManager.staticUpdate+=EventManager_staticUpdate;
		}
		private static void EventManager_staticUpdate() {
			if(instances.Count==0) return;
			if(!interactDisplayObject) return;
			currentTarget=GetTarget();
			if(currentTarget) interactDisplayObject.transform.position=currentTarget.transform.position+Vector3.up*2;
			interactDisplayObject.SetActive(currentTarget);
		}

		static GameObject interactDisplayObject;

		[SerializeField] GameObject interactDisplayPrefab;
		[SerializeField] Material interactMaterial;

		[SerializeField] UnityEngine.Events.UnityEvent onInteract;

		SpriteRenderer spriteRenderer;
		Material defaultMaterial;

		float distanceToCharacter;

		private void Start() {
			instances.Add(this);
			if(!interactDisplayObject&&interactDisplayPrefab) {
				interactDisplayObject=Instantiate(interactDisplayPrefab);
				interactDisplayObject.SetActive(false);
			}

			spriteRenderer=GetComponent<SpriteRenderer>();
			if(spriteRenderer) defaultMaterial=spriteRenderer.material;

		}
		private void FixedUpdate() {
			distanceToCharacter=1;
			for(int i = 0;i<3;i++) {
				if(!EntityFriendly.friendlyList[i]) continue;
				float newDistance = Mathf.Abs(EntityFriendly.friendlyList[i].transform.position.x-transform.position.x);
				if(newDistance<distanceToCharacter) distanceToCharacter=newDistance;
			}

			bool isTarget = currentTarget==this;
			if(spriteRenderer) spriteRenderer.material=isTarget ? interactMaterial : defaultMaterial;

		}
		private void OnDestroy() {
			instances.Remove(this);
		}
		private void OnDisable() {
			distanceToCharacter=1;
		}

	}

}
