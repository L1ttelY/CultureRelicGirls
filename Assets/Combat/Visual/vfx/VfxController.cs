using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class VfxController:MonoBehaviour {

		public void Init(Vector2 position,int direction) {
			spriteRenderer=GetComponent<SpriteRenderer>();

			transform.position=position;
			spriteRenderer.flipX=direction==Direction.right;

			imageIndex=0;
			spriteRenderer.sprite=sprites[0];
			timeThisFrame=0;
		}

		[SerializeField] Sprite[] sprites;
		[SerializeField] float timePerFrame = 0.04f;

		int imageIndex;
		SpriteRenderer spriteRenderer;
		float timeThisFrame;

		private void Update() {
			timeThisFrame+=Time.deltaTime;
			if(timeThisFrame>timePerFrame) {
				imageIndex++;
				timeThisFrame-=timePerFrame;
				if(imageIndex>=sprites.Length) VfxPool.Store(this);
				else spriteRenderer.sprite=sprites[imageIndex];
			}

		}	

	}
}