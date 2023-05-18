using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class PathFinder:MonoBehaviour {

		[SerializeField] float pivotHeight = 0.5f;
		[SerializeField] float moveSpeed = 5;

		const float stairTime = 0.5f;

		FloorPath targetFloor;
		[field: SerializeField] public FloorPath currentFloor { get; private set; }
		float targetX;
		SpriteRenderer spriteRenderer;

		public bool inPosition { get; private set; }

		public void SetTarget(Vector2 position) {
			targetFloor=FloorPath.FloorFromY(position.y);
			targetX=position.x;
		}

		private void Start() {
			spriteRenderer=GetComponentInChildren<SpriteRenderer>();
			currentFloor=FloorPath.FloorFromY(transform.position.y);
			targetFloor=currentFloor;
			targetX=transform.position.x;
		}

		public void TeleportToTarget() {
			if(!targetFloor) {
				currentFloor=targetFloor;
				transform.position=new Vector3(targetX,-10000);
				previousX=targetX;
				previousInactive=true;
				return;
			}
			currentFloor=targetFloor;
			transform.position=new Vector3(targetX,targetFloor.y);
			previousX=targetX;
		}

		public bool moving { get; private set; }
		public bool arrived { get { return targetFloor==currentFloor&&targetX==transform.position.x; } }

		float previousX;
		float stairTimeCurrent;
		bool previousInactive;
		private void Update() {
			/*
				if(Input.GetMouseButtonDown(0)) {
					SetTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				}
				*/
			if(targetFloor==null) {

				//不在场内
				currentFloor=null;
				transform.position=new Vector3(targetX,-10000);
				previousX=targetX;
				previousInactive=true;

			} else {

				if(previousInactive) {
					previousInactive=false;
					TeleportToTarget();
				}

				bool visible = true;
				float deltaX = Time.deltaTime*moveSpeed;
				float currentX = transform.position.x;


				if(currentFloor!=targetFloor) {

					//找楼梯口
					float stairX = currentFloor.stairX;
					if(currentX<stairX-deltaX) currentX+=deltaX;
					else if(currentX<=stairX+deltaX) currentX=stairX;
					else currentX-=deltaX;

					if(stairX==currentX) {
						//走楼梯
						stairTimeCurrent+=Time.deltaTime;
						visible=false;
						if(stairTimeCurrent>stairTime) currentFloor=targetFloor;
					} else {
						stairTimeCurrent=0;
					}

				} else {

					if(currentX<targetX-deltaX) currentX+=deltaX;
					else if(currentX<=targetX+deltaX) currentX=targetX;
					else currentX-=deltaX;

				}
				transform.position=new Vector3(currentX,currentFloor.y+pivotHeight);

				spriteRenderer.color=visible ? Color.white : Color.clear;

				if(currentX>previousX) transform.localScale=new Vector3(1,1,1);
				else if(currentX<previousX) transform.localScale=new Vector3(-1,1,1);
				moving=currentX!=previousX;
				previousX=currentX;

			}

		}

	}

}