using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {


	public class UIController:MonoBehaviour {
		[SerializeField] bool hideUI;
		[SerializeField] GameObject pauseMenu;
		void Start() {
			GetComponentInChildren<CombatRewardUIController>(true).Init();
		}

		private void LateUpdate() {
			if(hideUI) {
				var list = GetComponentsInChildren<Graphic>(true);
				foreach(var i in list) i.color=Color.clear;
			}
		}

		bool paused;
		public void PauseClick() {
			paused=true;
		}
		public void ResumeClick() {
			paused=false;
		}
		public void QuitClick() {
			CombatController.instance.forceEnd=true;
			paused=false;
		}

		private void Update() {

			if(paused) {
				pauseMenu.SetActive(true);
				Time.timeScale=0;
			} else {
				pauseMenu.SetActive(false);
				Time.timeScale=1;
			}

		}

	}

}