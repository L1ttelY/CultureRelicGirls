using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class Player:MonoBehaviour {

		[SerializeField] Transform leftBound;
		[SerializeField] Transform rightBound;
		[SerializeField] float resetSpeed = 2000;
		float leftX;
		float rightX;

		RectTransform rect;
		Canvas canvas;

		public static Player instance;

		public float targetVelocity { get; private set; }

		void Start() {
			if(instance) Debug.Break();
			instance=this;

			rect=transform as RectTransform;
			canvas=GetComponentInParent<Canvas>();
		}
		private void OnDestroy() {
			instance=null;
		}

		int touchId = -1;

		void Update() {
			leftX=leftBound.transform.position.x;
			rightX=rightBound.transform.position.x;

			targetVelocity=0;

			Touch? activeTouch = null;

			if(touchId!=-1) {

				//����Ƿ���Ȼ��ס
				bool down = false;
				for(int i = 0;i<Input.touchCount;i++) {
					if(Input.touches[i].fingerId==touchId) {
						down=true;
						activeTouch=Input.touches[i];
						break;
					}
				}

				if(!down) touchId=-1;

			} else {

				//����Ƿ���
				for(int i = 0;i<Input.touchCount;i++) {
					Vector2 position = TouchToPosition(Input.touches[i]);

					Rect worldRect = Utility.GetWorldRect(rect);

					if(worldRect.Contains(position)) {
						activeTouch=Input.touches[i];
						touchId=Input.touches[i].fingerId;
						break;
					}

				}

			}
			if(activeTouch!=null) {

				float targetX = TouchToPosition((Touch)activeTouch).x;
				targetX=Mathf.Clamp(targetX,leftX,rightX);

				Vector3 position = rect.position;
				position.x=targetX;
				rect.position=position;

				targetX-=leftX;
				targetX/=(rightX-leftX);
				targetVelocity=Mathf.Lerp(-1,1,targetX);

			} else {

				targetVelocity=0;
				Vector3 targetPosition = rect.position;
				targetPosition.x=(leftX+rightX)*0.5f;
				rect.position=Vector3.MoveTowards(rect.position,targetPosition,Time.deltaTime*resetSpeed);

			}

		}

		Vector2 TouchToPosition(Touch touch) {

			Vector2 localPoint;

			localPoint=touch.position;
			Rect pixelRect = canvas.pixelRect;
			Vector2 normalized = new Vector2(localPoint.x/pixelRect.width,localPoint.y/pixelRect.height);
			Rect worldRect = Utility.GetWorldRect(canvas.transform as RectTransform);

			Vector2 result = worldRect.min+new Vector2(normalized.x*worldRect.size.x,normalized.y*worldRect.size.y);

			return result;

		}


	}

}