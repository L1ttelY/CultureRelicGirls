using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class StationController:MonoBehaviour {

		public void OnInteract() {

		}

		[Tooltip("�ڴ������Ӧ��վ̨, ���Զ�������д��վ̨��Ϣ")]
		[SerializeField] StationData stationData;

		private void OnValidate() {
			if(stationData==null) return;
			stationData.SetTarget(gameObject);
		}

	}

}

