using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuilderArmController:MonoBehaviour {

		static Dictionary<int,BuilderArmController> instances = new Dictionary<int,BuilderArmController>();
		[SerializeField] int index;

		Animator animator;
		PathFinder pathFinder;
		BuildingControllerBase targetBuilding;

		private void Start() {

			animator=GetComponent<Animator>();

			if(!instances.ContainsKey(index)) instances.Add(index,this);
			else instances[index]=this;

			pathFinder=GetComponent<PathFinder>();
			pathFinder.SetTarget(new Vector2(0,-10000));
			pathFinder.TeleportToTarget();

		}

		private void FixedUpdate() {

			if(targetBuilding.saveData.levelUpStatus.value!=0) targetBuilding=null;
			if(targetBuilding==null) {
				foreach(var i in BuildingControllerBase.instances) {
					var target = i.Value;

					if(target.saveData.levelUpStatus.value!=0) {
						bool taken = false;
						foreach(var j in instances) {
							if(j.Value.targetBuilding==target) {
								taken=true;
								break;
							}
						}
						if(!taken) {
							targetBuilding=target;
							break;
						}

					}

				}

				pathFinder.SetTarget(GetTargetPosition());
			}

			if(pathFinder.arrived) {

				if(targetBuilding==null) {
					animator.SetTrigger("stay");
				} else {
					animator.SetTrigger("work");
				}

			} else {
				animator.SetTrigger("move");
			}

		}

		Vector2 GetTargetPosition() {
			if(index>=BuilderController.instance.levelUpMax) return new Vector2(0,-100000);
			if(targetBuilding==null) {
				return BuilderController.instance.transform.position;
			} else {
				return targetBuilding.transform.position;
			}

		}

	}
}