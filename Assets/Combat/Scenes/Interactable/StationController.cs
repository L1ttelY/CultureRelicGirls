using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	public class StationController:MonoBehaviour {

		public static StationData lastStationVisited;

		[Tooltip("�ڴ������Ӧ��վ̨, ���Զ�������д��վ̨��Ϣ")]
		[SerializeField] StationData stationData;
		PlayerData.DataBool boundFlag;

		Animator animator;

		bool animationGoing;

		public void OnInteract() {
			if(boundFlag.value) {
				lastStationVisited=stationData;
				SceneManager.LoadScene("VehicleScene");
			} else if(!animationGoing) {
				animator.SetTrigger("start");
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
			animator.SetBool("unlocked",boundFlag.value);
		}

		private void OnValidate() {
			if(Application.isPlaying) return;
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

