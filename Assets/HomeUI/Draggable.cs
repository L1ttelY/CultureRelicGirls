using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Home {

	public class Draggable:MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler {

		public object content;
		[HideInInspector] public Sprite sprite;
		public PointerEventData dragData { get; private set; }

		public static Draggable dragged { get; private set; }

		private void OnDisable() {
			if(dragged==this) dragged=null;
		}

		public void OnPointerDown(PointerEventData eventData) {
			dragged=this;
			dragData=eventData;
		}
		public void OnPointerUp(PointerEventData eventData) {
			if(dragged!=this) return;
			Releasable.DoRelease(content);
			dragged=null;
		}

		public void OnPointerMove(PointerEventData eventData) {
			dragData=eventData;
		}
	}

}