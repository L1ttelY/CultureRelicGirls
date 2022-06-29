using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ResetAllZ {

	[MenuItem("GameObject/重置所有物体的Z")]
	static void Work() {

		Object[] transforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
		foreach(Object i in transforms) {
			if(i is RectTransform) continue;
			Vector3 position = (i as Transform).position;
			position.z=0;
			(i as Transform).position=position;
		}

	}

	[MenuItem("GameObject/为所有按钮增加音效")]
	static void Worrk() {


	}

}