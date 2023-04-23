using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public class CameraController:MonoBehaviour {

		public static CameraController instance { get; private set; }

		[SerializeField] float moveSpeed = 5;
		[SerializeField] bool isFilming;

		new Camera camera;

		private void Start() {
			camera=GetComponent<Camera>();
			PlayerData.PlayerDataController.Init();
			instance=this;
		}

		private void Update() {
			if(isFilming) {
				UpdateFilmingMode();
			} else {
				UpdateTargetX();
				UpdateSelfPosition();
			}
		}

		float targetX;

		void UpdateTargetX() {
			/*
			//调整摄像机左右，越大越左，越小越右
			const float cameraStaticOffset = 10;
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

			float xMax = EntityFriendly.leftestX+cameraSize*0.5f-1f;
			float xMin = EntityFriendly.rightestX;

			targetX=Mathf.Clamp(targetX,xMin,xMax);
			*/

			float centerX = 0.5f*(EntityFriendly.rightestX+EntityFriendly.leftestX);
			float fov = Camera.VerticalToHorizontalFieldOfView(camera.fieldOfView,camera.aspect);
			float cameraRadius = Mathf.Tan(Mathf.Deg2Rad*fov*0.5f)*Mathf.Abs(transform.position.z);

			float minX = EntityFriendly.rightestX-cameraRadius;
			float maxX = EntityFriendly.leftestX+cameraRadius;


			SortedDictionary<float,EntityBase> enemiesByDist = new SortedDictionary<float,EntityBase>();
			foreach(var i in EntityBase.entities) {
				if((!i)||(!(i is EntityEnemy))) continue;
				enemiesByDist.Add(Mathf.Abs(i.transform.position.x-centerX),i);
			}

			foreach(var i in enemiesByDist){
				float x = i.Value.transform.position.x;
				if(x<maxX+cameraRadius&&x>minX-cameraRadius){
					minX=Mathf.Max(minX,x-cameraRadius);
					maxX=Mathf.Min(maxX,x+cameraRadius);
				}
			}

			targetX=(minX+maxX)*0.5f;
		}


		void UpdateSelfPosition() {
			Vector3 position = transform.position;
			Vector3 targetPosition = position;

			float x = position.x;
			float speed = moveSpeed*Mathf.Abs(x-targetX);
			targetPosition.x=targetX;
			position=Vector2.MoveTowards(position,targetPosition,speed*Time.deltaTime);

			position.z=-4.8f;
			//调整摄像机前后
			position.y=CombatRoomController.currentRoom.transform.position.y+1.1f;
			//调整摄像机上下
			transform.position=position;
		}

		Vector3 filmingModeVelocity;
		const float filmingModeAcceleration = 6;
		const float filmingModeSpeed = 6;
		void UpdateFilmingMode() {

			Vector3 targetVelocity = Vector3.zero;
			if(Input.GetKey(KeyCode.RightArrow)) targetVelocity+=Vector3.right;
			if(Input.GetKey(KeyCode.LeftArrow)) targetVelocity+=Vector3.left;
			if(Input.GetKey(KeyCode.UpArrow)) targetVelocity+=Vector3.up;
			if(Input.GetKey(KeyCode.DownArrow)) targetVelocity+=Vector3.down;
			targetVelocity*=filmingModeSpeed;

			filmingModeVelocity=Vector3.MoveTowards(filmingModeVelocity,targetVelocity,Time.deltaTime*filmingModeAcceleration);

			transform.position+=filmingModeVelocity*Time.deltaTime;
		}

	}
}