using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class UIController:MonoBehaviour {

		public static UIController instance;
		Dictionary<string,UIModeBase> UIModes = new Dictionary<string,UIModeBase>();

		public void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			UIModeBase[] modes = GetComponentsInChildren<UIModeBase>(true);
			foreach(var i in modes) {
				i.Init();
				Debug.Log(i.gameObject.name);
			}

		}

		public void AddMode(string name,UIModeBase Mode) {
			UIModes.Add(name,Mode);
		}

		public void SwitchUIMode(UIModeBase targetMode) {

			foreach(var i in UIModes) {
				UIModeBase mode = i.Value;

				if(mode!=targetMode) mode.gameObject.SetActive(false);
				else mode.gameObject.SetActive(true);
			}

		}

	}

}