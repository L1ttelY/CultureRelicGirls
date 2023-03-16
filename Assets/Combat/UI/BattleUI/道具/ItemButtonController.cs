using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat {

	public class ItemButtonController:MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerExitHandler {

		public enum States {
			Unpressed,
			Pressed,
			Selecting
		}
		public States state { get; private set; }

		/// <summary>
		/// ½«ÂÖÅÌµÄ±àºÅÓ³Éäµ½¿ì½ÝÀ¸µÄ±àºÅ
		/// </summary>
		public static int MapIndex(int selectionIndex) {
			if(selectionIndex<instance.chosenIndex) return selectionIndex;
			else return selectionIndex+1;
		}

		[SerializeField] Image image;

		public static ItemButtonController instance;

		float pressTime;
		const float timeToStartSelect = 0.5f;

		int chosenIndex;

		public int pointerId { get; private set; }

		bool IsMouseOver(List<GameObject> hover) {
			foreach(var i in hover) if(gameObject==i) return true;
			return false;
		}

		public void OnPointerDown(PointerEventData eventData) {
			if(state==States.Unpressed) {
				state=States.Pressed;
				pointerId=eventData.pointerId;
			}
		}
		public void OnPointerUp(PointerEventData eventData) {

			var hoverList = eventData.hovered;

			if(state==States.Pressed) {
				if(IsMouseOver(hoverList)) {
					LoadoutController.GetHotBar(chosenIndex).InvokeUse();
					state=States.Unpressed;
				} else {
					state=States.Selecting;
				}
			}

			if(state==States.Selecting) {
				foreach(var i in hoverList) {
					ItemSelectionIconController selection = i.GetComponent<ItemSelectionIconController>();
					if(selection==null) continue;
					Select(selection.index);
					return;
				}
				state=States.Unpressed;
			}

		}
		public void OnPointerExit(PointerEventData eventData) {
			if(state==States.Pressed) state=States.Selecting;
		}

		public void Select(int originalIndex) {

			if(state!=States.Selecting) return;
			state=States.Unpressed;

			int newIndex = MapIndex(originalIndex);
			if(!LoadoutController.GetHotBar(newIndex)) return;
			chosenIndex=newIndex;
		}

		private void Start() {
			instance=this;
		}

		void Update() {

			LoadoutController.SetHotBar(0,ItemData.instances["heal"]);
			LoadoutController.SetHotBar(1,ItemData.instances["heal 1"]);

			if(state==States.Pressed) {
				pressTime+=Time.deltaTime;
				if(pressTime>timeToStartSelect) state=States.Selecting;
			} else pressTime=0;

			image.sprite=LoadoutController.GetHotBar(chosenIndex)?.sprite;
			image.color=(image.sprite==null) ? Color.clear : Color.white;

		}

	}

}