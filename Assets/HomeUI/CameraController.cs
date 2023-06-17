using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Home {

	public class CameraController:MonoBehaviour {

		new public Camera camera { get; private set; }
		[SerializeField] float minSize;
		[SerializeField] float maxSize;
		[SerializeField] BoxCollider2D constraint;

		public static CameraController instance;
		private void Start() {
			instance=this;
			camera=GetComponent<Camera>();
			PlayerData.PlayerDataController.Init();
		}

		public CameraFocus focus { get; private set; }
		public bool SetFocus(CameraFocus newFocus) {

			if(newFocus) {
				//开始聚焦	
				if(focus) return false;
				focusChanged=true;
				focus=newFocus;

			} else {
				//取消聚焦	
				if(!focus) return false;
				focusChanged=true;
				focus=newFocus;

			}

			return true;

		}


		//用于在聚焦中保留摄像头的位置和大小
		float cameraSize = 5;
		Vector2 cameraPosition;

		bool focusChanged;
		private void Update() {

			if(focus) {

				//摄像头聚焦
				MoveTowards(focus.focusPoint,focus.focusSize);
				resizing=false;
				dragging=false;

			} else if(focusChanged) {
				//摄像头取消聚焦
				if(MoveTowards(cameraPosition,cameraSize)) focusChanged=false;

			} else {
				//摄像头由玩家控制

				UpdateResize();
				UpdateDrag();
				ConstrainTransform();

				camera.orthographicSize=cameraSize;
				transform.position=cameraPosition;

				if(
					Application.platform==RuntimePlatform.WindowsPlayer||
					Application.platform==RuntimePlatform.OSXPlayer||
					Application.platform==RuntimePlatform.WindowsEditor||
					Application.platform==RuntimePlatform.OSXEditor
				)
					UpdateDesktopCamera();

			}
		}

		bool draggingMouse;
		Vector2 originalPositionMouse;
		void UpdateDesktopCamera() {
			//Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
			float scale = (camera.ScreenToWorldPoint(Vector3.zero)-camera.ScreenToWorldPoint(Vector3.one)).x;
			Vector2 mousePosition = -Input.mousePosition*scale;
			//drag	

			if(!Input.GetMouseButton(0)) {
				draggingMouse=false;

				const float moveSpeed = 15;
				if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow)) cameraPosition+=moveSpeed*Vector2.up*Time.deltaTime;
				if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.LeftArrow)) cameraPosition+=moveSpeed*Vector2.left*Time.deltaTime;
				if(Input.GetKey(KeyCode.S)||Input.GetKey(KeyCode.DownArrow)) cameraPosition+=moveSpeed*Vector2.down*Time.deltaTime;
				if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.RightArrow)) cameraPosition+=moveSpeed*Vector2.right*Time.deltaTime;

			} else {

				if(!draggingMouse) {
					draggingMouse=true;
					originalPositionMouse=mousePosition;
				} else {
					cameraPosition-=mousePosition-originalPositionMouse;
					originalPositionMouse=mousePosition;
				}

			}

			//resize
			const float scaleSpeed = -0.1f;
			float resizeFactor = 1+Input.mouseScrollDelta.y*scaleSpeed;
			Vector2 vectorToMouse = mousePosition-cameraPosition;
			vectorToMouse*=resizeFactor;
			cameraPosition=mousePosition-vectorToMouse;
			cameraSize*=resizeFactor;

		}


		bool dragging;
		Vector2 originalPosition;
		void UpdateDrag() {
			if(Input.touchCount!=1) {
				dragging=false;
			} else {

				Vector2 touchPosition = camera.ScreenToWorldPoint(Input.touches[0].position);

				if(!dragging) {
					dragging=true;
					originalPosition=touchPosition;
				} else {
					cameraPosition-=touchPosition-originalPosition;
				}

			}
		}

		bool resizing;
		Vector2 originalCenter;
		Vector2 originalOffset;
		float originalDistance;
		float originalSize;
		void UpdateResize() {

			if(Input.touchCount!=2) {
				resizing=false;
			} else {

				Vector2 point1 = camera.ScreenToWorldPoint(Input.touches[0].position);
				Vector2 point2 = camera.ScreenToWorldPoint(Input.touches[1].position);

				if(resizing) {

					float newDistance = (point1-point2).magnitude;

					float scaleFactor = originalDistance/newDistance;
					Vector2 newOffset = originalOffset*scaleFactor;
					Vector2 newPosition = originalCenter-newOffset;
					cameraPosition=newPosition;

					cameraSize=originalSize*scaleFactor;
				}

				resizing=true;
				originalCenter=(point1+point2)*0.5f;
				originalDistance=(point1-point2).magnitude;
				originalSize=cameraSize;
				originalOffset=originalCenter-cameraPosition;

			}
		}

		const float sizeConstrainSpeed = 10;
		const float positionConstrainSpeed = 20;
		void ConstrainTransform() {

			if(!resizing) {
				cameraSize=SoftConstrain(cameraSize,minSize,maxSize,Time.deltaTime*sizeConstrainSpeed);
			}

			if(!dragging) {
				Bounds bound = constraint.bounds;
				Vector2 position = cameraPosition;
				position.x=SoftConstrain(position.x,bound.min.x,bound.max.x,Time.deltaTime*positionConstrainSpeed);
				position.y=SoftConstrain(position.y,bound.min.y,bound.max.y,Time.deltaTime*positionConstrainSpeed);
				cameraPosition=new Vector3(position.x,position.y,-10);
			}
		}

		float SoftConstrain(float current,float min,float max,float deltaMultiplier) {



			if(current<min) {
				float delta = (min-current)*deltaMultiplier;
				current+=delta;
				if(current>min) current=min;
			} else if(current>max) {
				float delta = (current-max)*deltaMultiplier;
				current-=delta;
				if(current<max) current=max;
			}
			return current;

		}

		const float moveSpeed = 7;
		const float scaleSpeed = 7;
		//返回是否移动到位
		bool MoveTowards(Vector2 position,float size) {

			Vector2 positionNow = transform.position;
			float speed = (position-positionNow).magnitude*moveSpeed;
			positionNow=Vector2.MoveTowards(positionNow,position,speed*Time.deltaTime);
			transform.position=new Vector3(positionNow.x,positionNow.y);

			float sizeNow = camera.orthographicSize;
			float sizeSpeed = Mathf.Abs(size-sizeNow)*scaleSpeed;
			float deltaSize = sizeSpeed*Time.deltaTime;
			if(sizeNow<size-deltaSize) sizeNow+=deltaSize;
			else if(sizeNow<=size+deltaSize) sizeNow=size;
			else sizeNow-=deltaSize;
			camera.orthographicSize=sizeNow;

			bool arrive = true;
			if(Mathf.Abs(sizeNow-size)>0.01f) arrive=false;
			if((position-positionNow).sqrMagnitude>0.1f) arrive=false;
			return arrive;

		}

	}

}
