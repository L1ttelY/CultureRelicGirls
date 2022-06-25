using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[AddComponentMenu("其它/射弹被摧毁时产生特效")]
	public class CreateVfxOnDeactivate:MonoBehaviour {

		[SerializeField] GameObject prefab;
		private void OnDisable() {
			VfxPool.Create(prefab,transform.position,Direction.right);
		}

	}

}