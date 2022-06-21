using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class CameraController:MonoBehaviour {

		const float positionZ = -10;

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
		float cameraSize;
		Vector2 cameraPosition;

		bool focusChanged;
		private void Update() {

			if(focus) {

				//摄像头聚焦
				if(focusChanged) {
					if(MoveTowards(focus.transform.position,focus.focusSize)) focusChanged=false;
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

			}
		}

		bool dragging;
		Vector2 previousPosition;
		void UpdateDrag() {
			if(Input.touchCount!=1) {
				dragging=false;
			} else {

				Vector2 touchPosition = camera.ScreenToWorldPoint(Input.touches[0].position);

				if(!dragging) {
					previousPosition=touchPosition;
				} else {
					transform.position-=(Vector3)(touchPosition-previousPosition);
					previousPosition=touchPosition;
				}

			}
		}

		bool resizing;
		Vector2 point1;
		Vector2 point2;
		Vector2 originalCenter;
		Vector2 originalOffset;
		float originalDistance;
		float originalSize;
		void UpdateResize() {

			if(Input.touchCount!=2) {
				resizing=false;
			} else {

				if(!resizing) {
					resizing=true;
					point1=camera.ScreenToWorldPoint(Input.touches[0].position);
					point2=camera.ScreenToWorldPoint(Input.touches[1].position);
					originalCenter=(point1+point2)*0.5f;
					originalDistance=(point1-point2).magnitude;
					originalSize=camera.orthographicSize;
					originalOffset=originalCenter-(Vector2)transform.position;
				} else {

					Vector2 newPoint1 = camera.ScreenToWorldPoint(Input.touches[0].position);
					Vector2 newPoint2 = camera.ScreenToWorldPoint(Input.touches[1].position);
					Vector2 newCenter = (newPoint1+newPoint2)*0.5f;
					float newDistance = (newPoint1-newPoint2).magnitude;

					float scaleFactor = originalDistance/newDistance;
					Vector2 newOffset = originalOffset*scaleFactor;
					Vector2 newPosition = originalCenter-newOffset;
					transform.position=new Vector3(newPosition.x,newPosition.y,positionZ);

					camera.orthographicSize=originalSize*scaleFactor;
				}

			}
		}

		const float sizeConstrainSpeed = 10;
		const float positionConstrainSpeed = 20;
		void ConstrainTransform() {

			camera.orthographicSize=SoftConstrain(camera.orthographicSize,minSize,maxSize,Time.deltaTime*sizeConstrainSpeed);
			Bounds bound = constraint.bounds;
			Vector2 position = transform.position;
			position.x=SoftConstrain(position.x,bound.min.x,bound.max.x,Time.deltaTime*positionConstrainSpeed);
			position.y=SoftConstrain(position.y,bound.min.y,bound.max.y,Time.deltaTime*positionConstrainSpeed);
			transform.position=new Vector3();

		}

		float SoftConstrain(float current,float min,float max,float deltaMultiplier) {

			if(current<min) {
				float delta = sizeConstrainSpeed*(min-current)*deltaMultiplier;
				current+=delta;
			} else if(current>max) {
				float delta = sizeConstrainSpeed*(current-max)*deltaMultiplier;
				current-=delta;
			}
			return current;

		}

		const float moveSpeed = 1;
		const float scaleSpeed = 1;
		//返回是否移动到位
		bool MoveTowards(Vector2 position,float size) {

			Vector2 positionNow = transform.position;
			float speed = (position-positionNow).magnitude*moveSpeed;
			Vector2.MoveTowards(positionNow,position,speed*Time.deltaTime);
			transform.position=new Vector3(positionNow.x,positionNow.y,positionZ);

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
