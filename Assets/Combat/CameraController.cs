using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class CameraController:MonoBehaviour {

		public static CameraController instance { get; private set; }

		[SerializeField]float cameraSize = 16;
		[SerializeField] float moveSpeed=5;

		private void Start() {
			instance=this;
		}

		private void Update() {
			UpdateTargetX();
			UpdateSelfPosition();
		}

		float targetX;

		void UpdateTargetX() {

			targetX=(EntityFriendly.leftestX+EntityFriendly.rightestX)*0.5f+5;

			

			if(targetX>EntityFriendly.leftestX-1+16*0.5f) targetX=EntityFriendly.leftestX-1+16*0.5f;
			if(targetX<EntityFriendly.rightestX+1-16*0.5f) targetX=EntityFriendly.rightestX+1-16*0.5f;

		}

		void UpdateSelfPosition() {
			Vector3 position = transform.position;
			Vector3 targetPosition = position;

			float x = position.x;
			float speed = moveSpeed*Mathf.Abs(x-targetX);
			targetPosition.x=targetX;
			position=Vector2.MoveTowards(position,targetPosition,speed*Time.deltaTime);

			position.z=-10;
			transform.position=position;
		}

	}
}