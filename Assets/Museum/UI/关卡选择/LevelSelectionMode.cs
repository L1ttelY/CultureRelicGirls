using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Museum {
	public class LevelSelectionMode:UIModeBase {

		[SerializeField] GameObject[] levelStates;

		public static LevelSelectionMode instance { get; private set; }
		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		public static void EnterMode() { instance._EnterMode(); }
		void _EnterMode() {
			gameObject.SetActive(true);
			UIController.instance.SwitchUIMode(this);
		}

		public int selectedMap { get; private set; }

		public void OnSelectMap(int mapIndex) {
			selectedMap=mapIndex;
			foreach(var i in levelStates) i.SetActive(false);
			levelStates[mapIndex].SetActive(true);
		}


	}
}