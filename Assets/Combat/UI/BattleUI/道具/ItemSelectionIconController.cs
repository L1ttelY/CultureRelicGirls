using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat {

	public class ItemSelectionIconController:MonoBehaviour {

		[field: SerializeField] public int index { get; private set; }
		[SerializeField] Image image;

		void Update() {

			if(ItemButtonController.instance.state==ItemButtonController.States.Selecting) {
				ItemData selectedItem = LoadoutController.GetHotBar(ItemButtonController.MapIndex(index));
				if(selectedItem) {
					image.sprite=selectedItem.sprite;
					image.color=Color.white;
				} else {
					image.sprite=null;
					image.color=Color.clear;
				}
			} else {
				image.sprite=null;
				image.color=Color.clear;
			}

		}

	}

}
