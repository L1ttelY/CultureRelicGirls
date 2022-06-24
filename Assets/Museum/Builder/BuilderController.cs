using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuilderController:MonoBehaviour {

		[SerializeField] CharacterController character;
		PathFinder pathFinder;
		Animator animator;

		bool working;
		public static BuilderController instance;
		private void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;
			pathFinder=GetComponent<PathFinder>();
			animator=GetComponent<Animator>();
		}

		private void FixedUpdate() {

			

			animator.SetBool("moving",pathFinder.moving);
			animator.SetBool("working",working);
		}


		public int levelUpMax { get { return 1; } }

	}

}