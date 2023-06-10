using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResizeSprite {

	[MenuItem("GameObject/����������ͷ�ľ������ô�С")]
	static void DoResizeSprite(MenuCommand context) {
		GameObject target = context.context as GameObject;
		float cameraZ = Camera.main.transform.position.z;
		float defaultDist = -cameraZ;
		float selfDist = target.transform.position.z-cameraZ;

		target.transform.localScale=Vector3.one*selfDist/defaultDist;
	}

}
