using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class SceneChange:MonoBehaviour {
		[SerializeField] string sceneName;
		[SerializeField] string roomName;
		[SerializeField] string startObject;

		public void OnInteract() {
			CombatController.StartCombat(roomName,sceneName,startObject);
		}
	}

}
