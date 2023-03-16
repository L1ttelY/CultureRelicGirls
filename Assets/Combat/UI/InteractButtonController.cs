using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class InteractButtonController:MonoBehaviour {

		Image image;

		private void Start() {
			image=GetComponent<Image>();
		}

		private void Update() {
			if(Interactable.CanInteract()){
				image.color=Color.white;
			}else {
				image.color=Color.clear;
			}
		}

		public void OnPress(){
			Interactable.DoInteract();
		}

	}

}