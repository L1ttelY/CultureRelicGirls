using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VehicleScene {

	public class CollectibleController:MonoBehaviour, IPointerDownHandler {

		[SerializeField] string contentName;
		[SerializeField][TextArea] string content;
		[SerializeField] Text targetText;
		[SerializeField] Text targetNameText;

		static CollectibleController current;

		float selectedTime;
		private void Update() {
			if(isCurrent) selectedTime=Mathf.Clamp(selectedTime+Time.deltaTime,0f,0.3f);
			else selectedTime=Mathf.Clamp(selectedTime-Time.deltaTime,0f,0.3f);
			transform.localScale=Vector3.one*(1+selectedTime);
			transform.localRotation=Quaternion.Euler(0,0,selectedTime*5f);
		}

		void SetAsCurrent() {
			targetText.text=content;
			targetNameText.text=contentName;
			current=this;
		}
		void ClearCurrent() {
			targetText.text="";
			targetNameText.text="";
			current=null;
		}
		bool isCurrent => current==this;

		public void OnPointerDown(PointerEventData eventData) {
			if(isCurrent) ClearCurrent();
			else SetAsCurrent();
		}
	}

}