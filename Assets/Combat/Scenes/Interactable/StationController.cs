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
		PlayerData.DataBool boundFlag;

		private void Start() {
			boundFlag=PlayerData.StationUnlockData.instance.unlockedStatus[stationData];
			//if(boundFlag.value)
		}

		private void OnValidate() {
			if(Application.isPlaying) return;
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

