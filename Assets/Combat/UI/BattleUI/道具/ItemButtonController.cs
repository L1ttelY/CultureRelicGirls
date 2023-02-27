using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat {

	public class ItemButtonController:MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler {

		public enum States {
			Unpressed,
			Pressed,
			Selecting
		}
		States state;

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

		bool mouseOver;

		public void OnPointerDown(PointerEventData eventData) {
			if(state==States.Unpressed) state=States.Pressed;
		}
		public void OnPointerExit(PointerEventData eventData) {
			mouseOver=false;
			if(state==States.Pressed) state=States.Selecting;
		}
		public void OnPointerEnter(PointerEventData eventData) {
			mouseOver=true;
		}

		public void Select(int originalIndex) {

			Debug.Log(originalIndex);

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

			Debug.Log(state);

			if(state==States.Pressed) {
				pressTime+=Time.deltaTime;
				if(pressTime>timeToStartSelect) state=States.Selecting;
			} else pressTime=0;

			if(Input.GetMouseButtonUp(0)&&!ItemSelectionIconController.IsMouseOver()) {
				if(state==States.Selecting) state=States.Unpressed;

				if(mouseOver){
					LoadoutController.GetHotBar(chosenIndex).InvokeUse();
				}
			}

			image.sprite=LoadoutController.GetHotBar(chosenIndex)?.sprite;
			image.color=(image.sprite==null) ? Color.clear : Color.white;

		}

	}

}