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

		[Tooltip("�ڴ������Ӧ��վ̨, ���Զ�������д��վ̨��Ϣ")]
		[SerializeField] StationData stationData;

		private void OnValidate() {
			if (Application.isPlaying) return;
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

