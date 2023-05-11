using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat {

	public class ItemSelectionIconController:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		[field: SerializeField] public int index { get; private set; }
		[SerializeField] Image image;
		[SerializeField] GameObject highLight;

		void Update() {

			if(ItemButtonController.instance.state==ItemButtonController.States.Selecting) {
				ItemData selectedItem = LoadoutController.GetHotBar(ItemButtonController.MapIndex(index));
				if(selectedItem) {
					image.sprite=selectedItem.sprite;
					image.color=Color.white;
					highLight.SetActive(selected==this);
				} else {
					image.sprite=null;
					image.color=Color.clear;
					highLight.SetActive(false);
				}
			} else {
				image.sprite=null;
				image.color=Color.clear;
				highLight.SetActive(false);
				selected=null;
			}

		}

		public ItemData boundItem => LoadoutController.GetHotBar(ItemButtonController.MapIndex(index));

		public static ItemSelectionIconController selected { get; private set; }
		public void OnPointerEnter(PointerEventData eventData) {
			selected=this;
		}
		public void OnPointerExit(PointerEventData eventDAta) {
			if(selected==this) selected=null;
		}
	}

}
