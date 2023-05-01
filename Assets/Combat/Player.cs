using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class Player:MonoBehaviour {

		public static event Void ChargeEvent;
		public static event Void ActionSkillEvent;

		[SerializeField] Image manaBar;

		[SerializeField] Transform leftBound;
		[SerializeField] Transform rightBound;
		[SerializeField] float resetSpeed = 2000;
		float leftX;
		float rightX;

		RectTransform rect;
		Canvas canvas;

		public static Player instance;

		public float targetVelocity { get; private set; }

		float _mana;
		//0~125
		public float mana {
			get => _mana;
			set {
				_mana=value;
				_mana=Mathf.Clamp(_mana,0,125);
			}
		}

		/// <summary>
		/// 数值参考Direction类
		/// </summary>
		public int teamDirection { get; private set; }
		float directionChangeTime;
		const float directionChangeTimeNeeded = 1;

		void Start() {
			ActionButtonCommandReceiver.OnDown+=ButtonClick;
			ActionButtonCommandReceiver.OnUp+=ButtonUp;

			if(instance) Debug.Break();
			instance=this;

			rect=transform as RectTransform;
			canvas=GetComponentInParent<Canvas>();
		}
		private void OnDestroy() {
			ActionButtonCommandReceiver.OnDown-=ButtonClick;
			ActionButtonCommandReceiver.OnUp-=ButtonUp;
			instance=null;
		}

		int touchId = -1;

		void Update() {

			if(mana<25) mana=Mathf.Min(25,mana+10*Time.deltaTime);
			manaBar.fillAmount=mana/125f;

			leftX=leftBound.transform.position.x;
			rightX=rightBound.transform.position.x;

			UpdateTargetVelocity();

			UpdateDirectionChange();
			UpdatePress();
		}

		void UpdateTargetVelocity() {

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

		public bool isBlocking { get; private set; }
		public int chargeDirection { get; private set; }
		public void ButtonClick() {
			if(Mathf.Abs(targetVelocity)>0.5f) {
				//冲刺
				if(mana>=25) {
					mana-=25;
					chargeDirection=(int)Mathf.Sign(targetVelocity);
					ChargeEvent?.Invoke();
					lastPressDash=true;
				}
			} else {
				//格挡
				isBlocking=true;
				lastPressDash=false;
				unprocessedPress=true;
			}
		}
		public void ButtonUp() {
			isBlocking=false;
			unprocessedPress=true;
		}

		public void SkillClick(int id) {
			if(!EntityFriendly.friendlyList[id]) return;
			EntityFriendly.friendlyList[id].TryUseActionSkill();
		}

		public float dashCdProgress {
			get { return Mathf.Clamp01(mana/25); }
		}
		//public float skillCdProgress {
		//	get { return skilledCharacter.skillCdProgress; }
		//}

		//public EntityFriendly skilledCharacter;

		bool lastPressDash;
		bool unprocessedPress;
		float timeAfterPress;

		[SerializeField] float parryWindowStart = 0.1f;
		[SerializeField] float parryWindowEnd = 0.2f;
		[SerializeField] float timeInvincible = 0.3f;

		public bool isParry { get; private set; }
		public bool isInvincible { get; private set; }
		float timeAfterInvincible;
		public bool UseParry() {
			if(!isParry) return false;
			if(isInvincible) return true;

			isInvincible=true;
			mana=125;
			timeAfterInvincible=0;

			return true;
		}

		void UpdatePress() {

			if(isBlocking) {
				if(!isInvincible) mana-=Time.deltaTime*25;
				if(mana<=0) isBlocking=false;
				targetVelocity=0;
			}

			if(unprocessedPress) {
				unprocessedPress=false;
				timeAfterPress=0;
			}
			if(!lastPressDash) {
				if(!isBlocking) timeAfterPress=-1;
				else timeAfterPress+=Time.deltaTime;
			} else timeAfterPress+=Time.deltaTime;

			if(timeAfterPress>parryWindowStart&&timeAfterPress<parryWindowEnd) {
				isParry=true;
			} else isParry=false;

			timeAfterInvincible+=Time.deltaTime;
			if(timeAfterInvincible>timeInvincible) {
				isInvincible=false;
			}

		}

	}

}