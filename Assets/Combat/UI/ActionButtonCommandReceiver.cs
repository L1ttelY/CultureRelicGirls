using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButtonCommandReceiver:MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
	public void OnPointerDown(PointerEventData eventData) => isDown=true;
	public void OnPointerExit(PointerEventData eventData) => isDown=false;
	public void OnPointerUp(PointerEventData eventData) => isDown=false;

	private void Update() {
		if(Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.LeftControl)) isDown=true;
	}

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
