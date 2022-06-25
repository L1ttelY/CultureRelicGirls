using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[AddComponentMenu("����/�䵯���ݻ�ʱ������Ч")]
	public class CreateVfxOnDeactivate:MonoBehaviour {

		[SerializeField] GameObject prefab;
		private void OnDisable() {
			VfxPool.Create(prefab,transform.position,Direction.right);
		}

	}

}