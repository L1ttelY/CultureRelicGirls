using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {


	public class InteractableDisplayController:MonoBehaviour {

		[SerializeField] Sprite[] sprites;

		SpriteRenderer spriteRenderer;
		int _imageIndex;

		public int imageIndex {
			set {
				_imageIndex=value;
				if(spriteRenderer) spriteRenderer.sprite=sprites[value];
			}
		}

		void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		private void Update() {
			spriteRenderer.sprite=sprites[_imageIndex];
		}

	}

}
