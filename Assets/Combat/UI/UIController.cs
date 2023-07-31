using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {


	public class UIController:MonoBehaviour {
		[SerializeField] bool hideUI;
		[SerializeField] GameObject pauseMenu;
		[SerializeField] GameObject endGameAnimation;
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

		bool prvPaused;
		private void Update() {

			if(paused!=prvPaused) {
				if(paused) {
					pauseMenu.SetActive(true);
					Time.timeScale=0;
				} else {
					pauseMenu.SetActive(false);
					Time.timeScale=1;
				}
				prvPaused=paused;
			}


			if(CombatController.instance.gameEnd) endGameAnimation.SetActive(true);

		}

	}

}