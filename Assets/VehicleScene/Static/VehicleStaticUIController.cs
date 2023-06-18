using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Home;

namespace VehicleScene {

	public class VehicleStaticUIController:MonoBehaviour {

		[SerializeField] Text uiNameText;
		[SerializeField] GameObject returnButton;

		public bool stopPop;
		public static event EventHandler OnReturn;

		public void OnReturnClick() {
			stopPop=false;
			OnReturn?.Invoke(this);
			if(stopPop) return;
			HomeUIStackManager.instance.TryPopUI();
		}

		private void Update() {
			uiNameText.text=HomeUIStackManager.instance.activeUI.Item1.gameObject.name;
			returnButton.gameObject.SetActive(HomeUIStackManager.instance.CanPopUI());
		}


	}

}
