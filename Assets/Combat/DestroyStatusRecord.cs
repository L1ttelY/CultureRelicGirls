using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class DestroyStatusRecord:MonoBehaviour {

		public static void ReviveAllDestroyedObjects() {
			destroyList.Clear();
		}

		static HashSet<string> destroyList = new HashSet<string>();

		private void Awake() {
			if(destroyList.Contains(Utility.GenerateNameFromGameObject(gameObject))) Destroy(gameObject);
		}
		private void OnDestroy() {
			if(!destroyList.Contains(Utility.GenerateNameFromGameObject(gameObject)))
				destroyList.Add(Utility.GenerateNameFromGameObject(gameObject));
		}

	}

}