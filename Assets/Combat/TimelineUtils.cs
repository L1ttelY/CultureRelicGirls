using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class TimelineUtils:MonoBehaviour {

		[Tooltip("是否停止玩家动作")]
		[SerializeField] bool stopPlayer;
		[Tooltip("是否将有房角色移动到指定位置")]
		[SerializeField] bool dragPlayer;

		[Tooltip("不要在timeline中更改, 开启dragPlayer后玩家会被拉到这个位置")]
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