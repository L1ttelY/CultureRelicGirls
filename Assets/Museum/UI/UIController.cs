using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class UIController:MonoBehaviour {

		public static UIController instance;
		Dictionary<string,UIModeBase> UIModes = new Dictionary<string,UIModeBase>();
		public static UIModeBase currentMode{ get; private set; }

		public void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			UIModeBase[] modes = GetComponentsInChildren<UIModeBase>(true);
			foreach(var i in modes) {
				i.Init();
			}
			EmptyMode.EnterMode();

		}

		public void AddMode(string name,UIModeBase Mode) {
			UIModes.Add(name,Mode);
		}

		public void SwitchUIMode(UIModeBase targetMode) {

			currentMode=targetMode;

			foreach(var i in UIModes) {
				UIModeBase mode = i.Value;

				if(mode!=targetMode) mode.gameObject.SetActive(false);
				else mode.gameObject.SetActive(true);
			}

		}

	}

}