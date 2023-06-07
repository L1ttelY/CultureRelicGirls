using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class StationController:MonoBehaviour {

		public void OnInteract() {

		}

		[Tooltip("在此输入对应的站台, 会自动将数据写入站台信息")]
		[SerializeField] StationData stationData;

		private void OnValidate() {
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

