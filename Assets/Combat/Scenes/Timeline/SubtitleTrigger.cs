using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Combat {

	[System.Serializable]
	public class SubtitleElement {
		public int targetId;
		public string content;
		public float delay;
	}

	public class SubtitleTrigger:MonoBehaviour {

		bool activated;
		float timer;
		int index;

		[SerializeField] SubtitleElement[] content;

		private void OnTriggerEnter2D(Collider2D collision) {
			if(activated) return;
			activated=true;
		}

		public void OnInteract()
        {
			if (activated) return;
			activated = true;

		}

		private void Update() {

			if(!activated) return;

			timer+=Time.deltaTime;
			if(index<content.Length) {
				SubtitleElement current = content[index];
				if(timer>current.delay) {
					timer-=current.delay;
					SubtitleController.instances[current.targetId].PushSubtitle(current.content);
					index++;
				}
			}

		}

	}

}