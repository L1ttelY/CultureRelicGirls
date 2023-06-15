using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class ChestAnimation:MonoBehaviour {

		[SerializeField] Sprite[] spriteSequence;
		[SerializeField] float timePerFrame;
		[SerializeField] string message;

		SpriteRenderer spriteRenderer;

		int imageIndex;
		bool started;
		float timeThisFrame;

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		public void OnInteract() {
			if(started) return;
			started=true;
			SubtitleController.instance.PushSubtitle(message);
		}

		private void Update() {
			if(!started) return;
			timeThisFrame+=Time.deltaTime;

			if(imageIndex<spriteSequence.Length-1&&timeThisFrame>timePerFrame) {
				timeThisFrame-=timePerFrame;
				imageIndex++;
			}
			spriteRenderer.sprite=spriteSequence[imageIndex];

		}
	}

}
