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

		[SerializeField] Transform startTransform;
		[SerializeField] float _initialX;
		public float initialX {
			get {
				if(startTransform) return startTransform.position.x;
				return _initialX;
			}
		}

		void Start() {

			if(gameObject.name==CombatController.startRoom) currentRoom=this;

			if(endX<startX) {
				//交换endX和startX
				float t = startX;
				startX=endX;
			}

		}

		bool prvActivate=true;
		Dictionary<GameObject,bool> activateBeforeDeactivate = new Dictionary<GameObject,bool>();

		void Update() {

			if(prvActivate!=isCurrentRoom) {

				if(isCurrentRoom) {
					//全部恢复激活状态
					for(int i = 0;i<transform.childCount;i++) {
						GameObject go = transform.GetChild(i).gameObject;
						if(go.GetComponent<DoNotDeactivate>()) continue;
						if(!activateBeforeDeactivate.ContainsKey(go)) continue;
						go.SetActive(activateBeforeDeactivate[go]);

					}

				} else {
					//全部关闭
					activateBeforeDeactivate.Clear();
					for(int i = 0;i<transform.childCount;i++) {
						GameObject go = transform.GetChild(i).gameObject;
						if(go.GetComponent<DoNotDeactivate>()) continue;
						activateBeforeDeactivate.Add(go,go.activeSelf);
						go.SetActive(false);
					}

				}

			}

			prvActivate=isCurrentRoom;

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