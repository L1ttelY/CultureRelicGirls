using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	public class StationController:MonoBehaviour {

		public static StationData lastStationVisited;

		public void OnInteract() {
			lastStationVisited=stationData;
			SceneManager.LoadScene("VehicleScene");
		}

		[Tooltip("在此输入对应的站台, 会自动将数据写入站台信息")]
		[SerializeField] StationData stationData;

		private void OnValidate() {
			if (Application.isPlaying) return;
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

