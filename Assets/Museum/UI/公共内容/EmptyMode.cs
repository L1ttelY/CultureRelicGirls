using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class EmptyMode:UIModeBase {

		[SerializeField] GameObject redDot;

		static EmptyMode instance;
		public override void Init() {
			base.Init();
			if(instance) Debug.Log("Duplicate");
			instance=this;
		}

		private void FixedUpdate() {
			var statusArr = PlayerData.PlayerDataRoot.instance.storyStatus;
			bool hasNewStory = false;
			for(int i = 0;i<statusArr.Length;i++) {
				if(statusArr[i].value==1) hasNewStory=true;
			}
			redDot.SetActive(hasNewStory);
		}

		public static void EnterMode() { instance._EnterMode(); }

		void _EnterMode() { UIController.instance.SwitchUIMode(this); }

		public void OnGoCombatClick() {
			LevelSelectionMode.EnterMode();
		}
		public void OnGoStoryClick() {
			StoryMode.EnterMode();
		}

	}

}