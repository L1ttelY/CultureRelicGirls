using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class CombatRoomController:MonoBehaviour {

		public static CombatRoomController currentRoom { get; private set; }

		public static event EventHandler RoomChange;
		void OnRoomChange() => RoomChange?.Invoke(this);

		[field: SerializeField] public float startX { get; private set; }
		[field: SerializeField] public float endX { get; private set; }


		void Start() {

			if(gameObject.name==CombatController.startRoom) currentRoom=this;

			if(endX<startX){
				//½»»»endXºÍstartX
				float t = startX;
				startX=endX;
			}
		}

		void Update() {
			
		}

		private void OnDrawGizmos() {
			Gizmos.color=Color.green;
			Gizmos.DrawLine(new Vector3(startX,transform.position.y),new Vector3(startX,transform.position.y+1));
			Gizmos.color=Color.red;
			Gizmos.DrawLine(new Vector3(endX,transform.position.y),new Vector3(endX,transform.position.y+1));
		}

		public bool isCurrentRoom => this==currentRoom;

		public void GoToRoom() {
			currentRoom=this;
			OnRoomChange();
		}



	}

}