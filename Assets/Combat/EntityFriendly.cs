using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityFriendly:EntityBase {

		//static update
		[RuntimeInitializeOnLoadMethod]
		static void SubscribeStaticEvents() {
			EventManager.staticUpdate+=StaticUpdate;
		}
		static void StaticUpdate() { //¹¥»÷¾àÀë£¿

			float originalLeftest = leftestX;
			float originalRightest = rightestX;
			bool friendlyLeft = false;

			leftestX=float.MaxValue;
			rightestX=float.MinValue;

			foreach(var i in entities) {
				if(i is EntityFriendly) {

					leftestX=Mathf.Min(i.transform.position.x,leftestX);
					rightestX=Mathf.Min(i.transform.position.x,leftestX);
					friendlyLeft=true;

				}
			}

			if(!friendlyLeft){
				leftestX=originalLeftest;
				rightestX=originalRightest;
			}

		}

		public static float leftestX { get; private set; }
		public static float rightestX { get; private set; }

		[SerializeField] int positionInTeam;

		public static EntityFriendly playerControlled;
		public static List<EntityFriendly> friendlyList = new List<EntityFriendly>();

		const float distancePerCharacter = 1;
		const float distanceTolerence = 0.3f;

		protected override void Start() {
			base.Start();
			if(positionInTeam==0) playerControlled=this;
			while(friendlyList.Count<=positionInTeam) friendlyList.Add(null);
			friendlyList[positionInTeam]=this;
		}

		protected override void StateMove() {
			base.StateMove();

			velocity.y=0;

			float buffedSpeed = maxSpeed*speedBuff;
			float buffedAcceleration = acceleration*speedBuff;

			Vector2 position = previousPosition;

			float targetVelocity;
			float deltaSpeed = buffedAcceleration*Time.deltaTime;

			int previousIndex=-1;
			EntityFriendly previousEntity=null;
			for(int comparisonIndex = positionInTeam-1;comparisonIndex>=0;comparisonIndex--) {
				if(friendlyList[comparisonIndex]) {
					previousEntity=friendlyList[comparisonIndex];
					previousIndex=comparisonIndex;
					break;
				}
			}


			if(!previousEntity) {
				targetVelocity=buffedSpeed*Player.instance.targetVelocity;

			} else {

				float targetPosition = 0;
				targetPosition=previousEntity.transform.position.x-(positionInTeam-previousIndex)*distancePerCharacter;

				float decelerateDistance = 0.5f*velocity.x*velocity.x/buffedAcceleration;
				if(decelerateDistance<distanceTolerence) decelerateDistance=distanceTolerence;
				targetVelocity=buffedSpeed*(targetPosition>position.x ? 1 : -1);
				if(Mathf.Abs(targetPosition-position.x)<decelerateDistance) {
					float distance = Mathf.Abs(targetPosition-position.x);

					targetVelocity=0;
					if(distance<distanceTolerence) targetVelocity=0;

					if(targetVelocity>buffedSpeed) targetVelocity=buffedSpeed;
					targetVelocity=targetVelocity*(targetPosition>position.x ? 1 : -1);
				}

			}

			if(Mathf.Abs(targetVelocity-velocity.x)<deltaSpeed) velocity.x=targetVelocity;
			else if(targetVelocity<velocity.x) velocity.x-=deltaSpeed;
			else velocity.x+=deltaSpeed;

			position.x+=velocity.x*Time.deltaTime;
			position.y=0;
			transform.position=position;
			previousPosition=position;

			UpdateAttack();

		}

		protected override void OnDeath() {
			base.OnDeath();

			FriendlyCorpseController.Create(transform,spriteRenderer.sprite);

			Destroy(gameObject);
		}

	}
}