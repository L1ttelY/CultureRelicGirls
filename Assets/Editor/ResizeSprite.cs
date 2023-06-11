using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResizeSprite {

	[MenuItem("GameObject/按照与摄像头的距离设置大小")]
	static void DoResizeSprite(MenuCommand context) {
		GameObject target = context.context as GameObject;
		float cameraZ = Camera.main.transform.position.z;
		float defaultDist = -cameraZ;
		float selfDist = target.transform.position.z-cameraZ;

		target.transform.localScale=Vector3.one*selfDist/defaultDist;
	}

}
