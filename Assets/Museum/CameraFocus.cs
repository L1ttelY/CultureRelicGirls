using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	[AddComponentMenu("博物馆/摄像头聚焦对象")]
	public class CameraFocus:MonoBehaviour {
		[field: SerializeField] public float focusSize { get; private set; }
	}
}