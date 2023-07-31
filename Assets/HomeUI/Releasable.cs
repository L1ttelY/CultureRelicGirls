using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Home {

	public class Releasable:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		public static Releasable mouseOver { get; private set; }
		public static bool DoRelease(object content) {
			if(!mouseOver) return false;
			mouseOver.OnRelease?.Invoke(content,mouseOver);
			return mouseOver.releaseResult;
		}

		public bool releaseResult;
		[SerializeField] UnityEvent<object,Releasable> OnRelease;

		public void OnPointerEnter(PointerEventData eventData) {
			mouseOver=this;
		}

		public void OnPointerExit(PointerEventData eventData) {
			if(mouseOver==this) mouseOver=null;
		}

		private void OnDisable() {
			if(mouseOver==this) mouseOver=null;
		}

		private void Update() {
			if(!Draggable.dragged) mouseOver=null;
		}

	}

}