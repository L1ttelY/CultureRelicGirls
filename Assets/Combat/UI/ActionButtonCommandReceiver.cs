using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtonCommandReceiver:MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	public void OnPointerDown(PointerEventData eventData) => isDown=true;
	public void OnPointerExit(PointerEventData eventData) => isDown=false;
	public void OnPointerUp(PointerEventData eventData) => isDown=false;
	bool _isDown;
	public bool isDown {
		get => _isDown;
		private set {
			if(value==_isDown) return;
			_isDown=value;
			if(value) OnDown();
			else OnUp();
		}
	}
	public static event Void OnDown;
	public static event Void OnUp;

}
