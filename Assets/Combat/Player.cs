using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class Player:MonoBehaviour {

		public static event Void ChargeEvent;
		public static event Void ActionSkillEvent;

		[SerializeField] Transform leftBound;
		[SerializeField] Transform rightBound;
		[SerializeField] float resetSpeed = 2000;
		float leftX;
		float rightX;

		RectTransform rect;
		Canvas canvas;

		public static Player instance;

		public float targetVelocity { get; private set; }

		/// <summary>
		/// 数值参考Direction类
		/// </summary>
		public int teamDirection { get; private set; }
		float directionChangeTime;
		const float directionChangeTimeNeeded = 1;

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

			if(timeAfterDash<dashCd) timeAfterDash+=Time.deltaTime;

			leftX=leftBound.transform.position.x;
			rightX=rightBound.transform.position.x;

			targetVelocity=0;

			Touch? activeTouch = null;

			if(touchId!=-1) {

				//检测是否仍然按住
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

				//检测是否按下
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

			if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)) {
				targetVelocity=0;
				if(Input.GetKey(KeyCode.A)) targetVelocity-=1;
				if(Input.GetKey(KeyCode.D)) targetVelocity+=1;
			}

			UpdateDirectionChange();

		}

		void UpdateDirectionChange() {

			bool inChange = (targetVelocity>0)==(teamDirection==Direction.left);
			if(targetVelocity==0) inChange=false;

			if(EntityFriendly.HasTarget()) directionChangeTime=0;
			if(!inChange) directionChangeTime=0;
			else directionChangeTime+=Time.deltaTime;

			if(directionChangeTime>=directionChangeTimeNeeded) {
				
				teamDirection=targetVelocity<0 ? Direction.left : Direction.right;
				directionChangeTime=0;
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

		public int chargeDirection { get; private set; }
		public void ButtonClick() {
			Debug.Log(targetVelocity);
			if(Mathf.Abs(targetVelocity)>0.98f) {
				if(timeAfterDash>=dashCd) {
					chargeDirection=(int)Mathf.Sign(targetVelocity);
					ChargeEvent?.Invoke();
					timeAfterDash=0;
				}
			}
		}

		public void SkillClick(int id) {
			if(!EntityFriendly.friendlyList[id]) return;
			EntityFriendly.friendlyList[id].ActionSkill();
		}

		float timeAfterDash;
		[SerializeField] float dashCd = 10;

		public float dashCdProgress {
			get { return timeAfterDash/dashCd; }
		}
		//public float skillCdProgress {
		//	get { return skilledCharacter.skillCdProgress; }
		//}

		//public EntityFriendly skilledCharacter;

	}

}