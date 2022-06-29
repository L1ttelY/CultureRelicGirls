using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuilderArmController:MonoBehaviour {

		public static Dictionary<int,BuilderArmController> instances = new Dictionary<int,BuilderArmController>();
		[field: SerializeField] public int index { get; private set; }

		Animator animator;
		public PathFinder pathFinder { get; private set; }
		public BuildingControllerBase targetBuilding { get; private set; }

		private void Start() {

			animator=GetComponent<Animator>();

			if(!instances.ContainsKey(index)) instances.Add(index,this);
			else instances[index]=this;

			pathFinder=GetComponent<PathFinder>();
			pathFinder.SetTarget(new Vector2(0,-10000));
			pathFinder.TeleportToTarget();

			animator.speed=Random.Range(0.8f,1.2f);

		}

		private void FixedUpdate() {

			if(targetBuilding!=null&&targetBuilding.saveData.levelUpStatus.value==0) targetBuilding=null;
			if(index<BuilderController.instance.levelUpMax&&targetBuilding==null) {
				foreach(var i in BuildingControllerBase.instances) {
					var crr = i.Value;

					if(crr.saveData.levelUpStatus.value!=0) {

						bool taken = false;
						foreach(var j in instances) {
							if(j.Value.targetBuilding==crr) {
								taken=true;
								break;
							}
						}

						if(!taken) {
							targetBuilding=crr;
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
				if(index==2) return BuilderController.instance.transform.position+Vector3.right*0.5f;
				if(index==1) return BuilderController.instance.transform.position+Vector3.left*0.5f;
				return BuilderController.instance.transform.position;
			} else {
				return targetBuilding.transform.position;
			}

		}

	}
}