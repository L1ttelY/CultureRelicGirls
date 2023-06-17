using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class SceneChange:MonoBehaviour {
		[SerializeField] string sceneName;
		[SerializeField] string roomName;
		[SerializeField] string startObject;

		[SerializeField] CombatEntry targetEntry;

		private void OnValidate() {
			if(targetEntry!=null){
				sceneName=targetEntry.sceneName;
				roomName=targetEntry.roomName;
				startObject=targetEntry.startObjectName;
			}
		}

		public void OnInteract() {
			CombatController.StartCombat(sceneName,roomName,startObject);
		}
	}

}
