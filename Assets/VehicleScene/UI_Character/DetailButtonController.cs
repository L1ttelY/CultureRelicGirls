using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VehicleScene {
	public class DetailButtonController:MonoBehaviour, IPointerDownHandler {

		[SerializeField] GameObject target;

		public void OnPointerDown(PointerEventData eventData) {
			target.SetActive(true);
		}
	}

}