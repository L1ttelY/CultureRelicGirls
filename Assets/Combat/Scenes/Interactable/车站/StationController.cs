using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	public class StationController:MonoBehaviour {

		public static string lastStationVisited = "";
		public static StationData lastStationVisitedData =>
			lastStationVisited.Length==0 ? null : StationData.instances[lastStationVisited];

		[Tooltip("在此输入对应的站台, 会自动将数据写入站台信息")]
		[SerializeField] StationData stationData;
		PlayerData.DataBool boundFlag;

		Animator animator;

		bool animationGoing;

		public void OnInteract() {
			if(boundFlag.value) {
				lastStationVisited=stationData.name;
				SceneManager.LoadScene("VehicleScene");
			} else if(!animationGoing) {
				if(animator) animator.SetTrigger("start");
			}
		}

		private void Start() {
			animator=GetComponent<Animator>();
			boundFlag=PlayerData.StationUnlockData.instance.unlockedStatus[stationData.name];
		}

		public void OnAnimationFinish() {
			animationGoing=false;
			boundFlag.value=true;
		}

		private void Update() {
			if(animator) animator.SetBool("unlocked",boundFlag.value);
		}

		private void OnValidate() {
			if(Application.isPlaying) return;
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

