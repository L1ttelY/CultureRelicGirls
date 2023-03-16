using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class Pathway:MonoBehaviour {

		[SerializeField] GameObject otherSide;

		public void GoThrough() {

			CombatRoomController targetRoom = otherSide.GetComponentInParent<CombatRoomController>();
			if(targetRoom==null) return;

			CombatController.startX=otherSide.transform.position.x;	
			targetRoom.GoToRoom();

			Debug.Log("!!!");

		}

	}

}
