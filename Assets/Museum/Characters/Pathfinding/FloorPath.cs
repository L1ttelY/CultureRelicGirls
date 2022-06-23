using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class FloorPath:MonoBehaviour {

		[field: SerializeField] public int floorIndex { get; private set; }//楼层数 从0开始 越小的楼层数对应的y越小

		public float y { get { return transform.position.y; } }
		public float stairX { get { return transform.position.x; } }

		[field: SerializeField] public float leftX { get; private set; }
		[field: SerializeField] public float rightX { get; private set; }

		public static FloorPath[] floorPaths = new FloorPath[3];
		public static FloorPath FloorFromY(float y) {

			int maxIndex = 0;
			foreach(var i in floorPaths) {
				if(i.y<=y) maxIndex=Mathf.Max(i.floorIndex,maxIndex);
			}
			return floorPaths[maxIndex];
		}

		private void Start() {
			floorPaths[floorIndex]=this;
		}

		void OnDrawGizmos() {
			Vector3 pos1 = transform.position;
			Vector3 pos2 = transform.position;
			pos1.x=leftX;
			pos2.x=rightX;
			Gizmos.DrawLine(pos1,pos2);

			Gizmos.DrawCube(transform.position,Vector3.one*0.2f);
		}

	}


}