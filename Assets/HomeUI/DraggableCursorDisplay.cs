using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Home;
using UnityEngine.UI;

namespace Home {

	public class DraggableCursorDisplay:MonoBehaviour {

		RectTransform parent;
		RectTransform rectTransform;

		Draggable prvDragged;
		Image image;
		private void Start() {
			rectTransform=transform as RectTransform;
			parent=transform.parent as RectTransform;
			image=GetComponent<Image>();
		}

		private void Update() {

			if(Draggable.dragged) {
				if(!prvDragged) transform.position=Draggable.dragged.transform.position;

				image.sprite=Draggable.dragged.sprite;
				if(Draggable.dragged.dragData!=null) {
					Vector2 localPoint;
					RectTransformUtility.ScreenPointToLocalPointInRectangle(
						parent,
						Draggable.dragged.dragData.position,
						Camera.main,
						out localPoint
					);
					rectTransform.anchoredPosition=localPoint;
				}
				image.color=image.sprite ? Color.white : Color.clear;
			} else {
				image.color=Color.clear;
			}

			prvDragged=Draggable.dragged;
		}

	}

}