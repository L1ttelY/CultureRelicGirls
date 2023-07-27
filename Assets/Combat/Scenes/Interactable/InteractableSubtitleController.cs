using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Combat {
	public class InteractableSubtitleController:MonoBehaviour {

		[TextArea][SerializeField] string textUsable;
		[TextArea][SerializeField] string textLocked = "需要找到对应的钥匙才能使用";
		[TextArea][SerializeField] string textSuppressed = "击杀附近的怪物才能使用";

		const int targetSubtitle = 2;

		Interactable interactable;
		private void Start() {
			interactable=GetComponent<Interactable>();
		}
		private void Update() {
			if(interactable!=null) {
				if(interactable==Interactable.GetTarget()) {
					SubtitleController.instances[targetSubtitle].PushSubtitle(textUsable);
				} else if(interactable==Interactable.GetTargetFallback()) {
					switch(interactable.status) {
					case Interactable.InteractableStatus.Locked:
						SubtitleController.instances[targetSubtitle].PushSubtitle(textLocked);
						break;
					case Interactable.InteractableStatus.Supressed:
						SubtitleController.instances[targetSubtitle].PushSubtitle(textSuppressed);
						break;
					}
				}
			}
		}

	}
}