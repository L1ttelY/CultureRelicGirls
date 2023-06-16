using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class TimelineUtils:MonoBehaviour {

		[Tooltip("�Ƿ�ֹͣ��Ҷ���")]
		[SerializeField] bool stopPlayer;
		[Tooltip("�Ƿ��з���ɫ�ƶ���ָ��λ��")]
		[SerializeField] bool dragPlayer;

		[Tooltip("��Ҫ��timeline�и���, ����dragPlayer����һᱻ�������λ��")]
		[SerializeField] GameObject dragPlayerTo;

		public static bool shouldStop => instance ? (instance.stopPlayer||instance.dragPlayer) : false;

		public static TimelineUtils instance;

		bool dragPrv;

		private void Start() {
			instance=this;
		}


		private void Update() {

			if(dragPlayer) {
				List<EntityFriendly> dragTarget = EntityFriendly.friendlyList.FindAll((x) => { return x; });
				Vector3 dragPosition = dragPlayerTo.transform.position-(dragTarget.Count-1)*Vector3.left*0.5f;
				for(int i = 0;i<dragTarget.Count;i++) {
					dragTarget[i].transform.position=dragPosition+Vector3.right*i;
				}
			}

		}

	}

}