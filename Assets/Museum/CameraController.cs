using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class CameraController:MonoBehaviour {

		new public Camera camera { get; private set; }
		[SerializeField] float minSize;
		[SerializeField] float maxSize;
		[SerializeField] BoxCollider2D constraint;

		public static CameraController instance;
		private void Start() {
			instance=this;
			camera=GetComponent<Camera>();
		}

		CameraFocus focus;
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
				if(focusChanged) {
					if(MoveTowards(focus.focusPoint,focus.focusSize)) focusChanged=false;
				}
				resizing=false;

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

			}
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
