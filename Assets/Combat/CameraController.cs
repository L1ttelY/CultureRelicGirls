using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class CameraController:MonoBehaviour {

		public static CameraController instance { get; private set; }

		[SerializeField] float cameraSize = 16;
		[SerializeField] float moveSpeed = 5;

		private void Start() {
			instance=this;
		}

		private void Update() {
			UpdateTargetX();
			UpdateSelfPosition();
		}

		float targetX;


		void UpdateTargetX() {

			const float cameraStaticOffset = 7;
			targetX=(EntityFriendly.leftestX+EntityFriendly.rightestX)*0.5f+cameraStaticOffset;

			float originalTargetX = targetX;
			const float offsetRange = 12;
			const float offsetStrength = 0.2f;
			foreach(var entity in EntityBase.entities) {
				float x = entity.transform.position.x;
				if(Mathf.Abs(x-originalTargetX)<offsetRange) {
					float thisTimeStrength = x<originalTargetX ? 2 : 1;
					thisTimeStrength*=offsetStrength;
					targetX+=thisTimeStrength*Mathf.Sqrt(Mathf.Abs(x-originalTargetX))*Mathf.Sign(x-originalTargetX);
				}
			}

			float xMax = EntityFriendly.leftestX+cameraSize*0.5f-1;
			float xMin = EntityFriendly.rightestX;

			targetX=Mathf.Clamp(targetX,xMin,xMax);

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