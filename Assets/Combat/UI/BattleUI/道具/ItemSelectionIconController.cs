using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat {

	public class ItemSelectionIconController:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		[SerializeField] int index;
		[SerializeField] Image image;

		void Start() {
			instances.Add(this);
		}

		void OnDestroy() {
			instances.Remove(this);
		}

		void Update() {
			ItemData selectedItem = LoadoutController.GetHotBar(ItemButtonController.MapIndex(index));
			if(selectedItem) image.sprite=selectedItem.sprite;
			else image.sprite=null;

			if(Input.GetMouseButtonUp(0)) ItemButtonController.instance.Select(index);
		}

		void OnDisable() {
			mouseOverSelf=false;
		}

		public static HashSet<ItemSelectionIconController> instances = new HashSet<ItemSelectionIconController>();

		public static bool IsMouseOver() {
			foreach(var i in instances) {
				if(i&&i.mouseOverSelf) return true;
			}
			return false;
		}

		bool mouseOverSelf;
		public void OnPointerEnter(PointerEventData eventData) => mouseOverSelf=true;
		public void OnPointerExit(PointerEventData eventData) => mouseOverSelf=false;

	}

}
