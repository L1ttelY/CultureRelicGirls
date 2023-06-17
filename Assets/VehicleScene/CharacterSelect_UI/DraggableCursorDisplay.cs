using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Home;
using UnityEngine.UI;

namespace VehicleScene {

	public class DraggableCursorDisplay:MonoBehaviour {

		RectTransform parent;

		Draggable prvDragged;
		Image image;
		private void Start() {
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
					transform.position=localPoint;
				}
				image.color=Color.white;
			} else {
				image.color=Color.clear;
			}

			prvDragged=Draggable.dragged;
		}

	}

}