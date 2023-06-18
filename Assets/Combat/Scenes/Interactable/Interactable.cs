using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Interactable:MonoBehaviour {

		public enum InteractableStatus {
			Enabled = 0,   //可用
			Disabled = 1,  //已经消失
			Supressed = 2, //被怪物压制
			Locked = 3     //还未解锁
		}

		const float interactDistance = 1;

		static HashSet<Interactable> instances = new HashSet<Interactable>();
		public static bool CanInteract() {
			foreach(var i in instances) {
				if(!i.enabled) continue;
				if(i.distanceToCharacter<interactDistance) return true;
			}
			return false;
		}
		public static Interactable GetTarget() {
			if(!CanInteract()) return null;
			float minDistance = interactDistance;
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
		public static Interactable GetTargetFallback() {
			float minDistance = interactDistance;
			Interactable target = null;
			foreach(var i in instances) {
				if(!i.gameObject.activeInHierarchy) continue;
				if(i.status==InteractableStatus.Enabled) continue;
				if(i.enabled) continue;
				Transform transform = i.transform;
				float distanceToCharacter=1;
				for(int j = 0;j<3;j++) {
					if(!EntityFriendly.friendlyList[j]) continue;
					float newDistance = Mathf.Abs(EntityFriendly.friendlyList[j].transform.position.x-transform.position.x);
					if(newDistance<distanceToCharacter) distanceToCharacter=newDistance;
				}

				if(distanceToCharacter<minDistance) {
					minDistance=i.distanceToCharacter;
					target=i;
				}
			}

			return target;

		}
		public static void DoInteract() {
			if(!CanInteract()) return;
			GetTarget()?.onInteract?.Invoke();
		}

		public InteractableStatus status;

		static Interactable currentTarget;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			EventManager.staticUpdate+=EventManager_staticUpdate;
		}
		private static void EventManager_staticUpdate() {
			if(instances.Count==0) return;
			if(!interactDisplayObject) return;
			currentTarget=GetTarget();
			if(currentTarget) {
				interactDisplayObject.transform.position=currentTarget.transform.position+Vector3.up*2;
				displayScript.imageIndex=(int)InteractableStatus.Enabled;
				interactDisplayObject.SetActive(true);
			} else if(GetTargetFallback()) {
				currentTarget=GetTargetFallback();
				interactDisplayObject.transform.position=currentTarget.transform.position+Vector3.up*2;
				displayScript.imageIndex=(int)currentTarget.status;
				interactDisplayObject.SetActive(true);
			} else {
				interactDisplayObject.SetActive(false);

			}

		}

		static GameObject interactDisplayObject;
		static InteractableDisplayController displayScript;

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
				displayScript=interactDisplayObject.GetComponent<InteractableDisplayController>();
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
