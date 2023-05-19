using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class ConfirmationMode:UIModeBase {

		[SerializeField] Text text;

		public static ConfirmationMode instance { get; private set; }

		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		string message;
		Void callbackYes;
		Void callbackNo;
		public static void EnterMode(string message,Void callbackYes,Void callbackNo) {
			instance._EnterMode(message,callbackYes,callbackNo);
		}
		void _EnterMode(string message,Void callbackYes,Void callbackNo) {
			this.callbackYes=callbackYes;
			this.callbackNo=callbackNo;
			this.message=message;
			text.text=message;
			UIController.instance.SwitchUIMode(this);
		}

		public void OnClickYes() {
			EmptyMode.EnterMode();
			callbackYes?.Invoke();
		}
		public void OnClickNo() {
			EmptyMode.EnterMode();
			callbackNo?.Invoke();
		}
	}
	 
}