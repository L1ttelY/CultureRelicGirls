using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class ChestAnimation:MonoBehaviour {

		[SerializeField] Sprite[] spriteSequence;
		[SerializeField] float timePerFrame;
		[SerializeField] string message;
		[SerializeField] string flagName;

		SpriteRenderer spriteRenderer;
		Interactable interactable;

		int imageIndex;
		bool started;
		float timeThisFrame;

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
			if(flagName.Length==0) flagName=Utility.GenerateNameFromGameObject(gameObject);
			interactable=GetComponent<Interactable>();
			if(PlayerData.Flags.instance.HasFlag(flagName)) {
				interactable.enabled=false;
				spriteRenderer.sprite=spriteSequence[spriteSequence.Length-1];
			}
		}

		public void OnInteract() {
			if(started) return;
			started=true;
			SubtitleController.instances[0].PushSubtitle(message);
			PlayerData.Flags.instance.SetFlag(flagName);
			interactable.enabled=false;
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
