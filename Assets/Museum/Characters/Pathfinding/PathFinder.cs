using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class PathFinder:MonoBehaviour {

		[SerializeField] float pivotHeight = 0.5f;
		[SerializeField] float moveSpeed = 5;

		const float stairTime = 0.5f;

		FloorPath targetFloor;
		FloorPath currentFloor;
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

		public bool moving { get; private set; }

		float previousX;
		float stairTimeCurrent;
		private void Update() {

			bool visible = true;
			float deltaX = Time.deltaTime*moveSpeed;
			float currentX = transform.position.x;

			if(currentFloor!=targetFloor) {

				//ÕÒÂ¥ÌÝ¿Ú
				float stairX = currentFloor.stairX;
				if(currentX<stairX-deltaX) currentX+=deltaX;
				else if(currentX<=stairX+deltaX) currentX=stairX;
				else currentX-=deltaX;

				if(stairX==currentX) {
					//×ßÂ¥ÌÝ
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