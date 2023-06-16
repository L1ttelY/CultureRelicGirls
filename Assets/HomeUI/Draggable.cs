using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Home {

	public class Draggable:MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		public object content;
		public Sprite sprite;

		public static Draggable dragged{ get; private set; }

		public void OnPointerDown(PointerEventData eventData) {
			dragged=this;
		}
		public void OnPointerUp(PointerEventData eventData) {
			if(dragged!=this) return;
			Releasable.DoRelease(content);
			dragged=null;
		}

	}

}