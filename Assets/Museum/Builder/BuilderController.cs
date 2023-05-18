using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuilderController:CharacterController {

		public static BuilderController instance;
		protected override void Start() {
			base.Start();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		protected override void Update() {
			base.Update();
			if(working&&pathFinder.arrived) {
				float deltaX = targetArm.transform.position.x-transform.position.x;
				transform.localScale=deltaX>0 ? new Vector3(1,1,1) : new Vector3(-1,1,1);
			}
		}

		protected override void UpdateAnimator() {
			if(working) animator.SetBool("isWorking",working&&pathFinder.arrived);
			else animator.SetBool("isWorking",false);
			base.UpdateAnimator();
		}

		BuilderArmController targetArm;
		bool working;

		protected override Vector2 GetTargetPosition() {

			if(targetArm==null) {
				foreach(var i in BuilderArmController.instances) {
					var arm = i.Value;
					if(arm.targetBuilding!=null) {
						targetArm=arm;
						break;
					}
				}
			} else {
				if(targetArm.targetBuilding==null) targetArm=null;
			}

			if(saveData.healStatus.value==0&&saveData.levelUpStatus.value==0&&targetArm!=null) {
				working=true;
				if(
					Mathf.Abs(targetArm.transform.position.x-transform.position.x)<1&&
					targetArm.pathFinder.currentFloor==pathFinder.currentFloor
				) return transform.position;
				return targetArm.transform.position+Vector3.up+new Vector3(1,1);
			}
			Debug.Log("?");
			working=false;
			return base.GetTargetPosition();
		}

		public int levelUpMax {
			get {
				if(saveData.level.value>=5) return 3;
				if(saveData.level.value>=3) return 2;
				return 1;
			}
		}

	}

}