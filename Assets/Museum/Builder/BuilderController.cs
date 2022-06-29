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


		protected override void UpdateAnimator() {
			base.UpdateAnimator();
		}

		protected override Vector2 GetTargetPosition() {

			return base.GetTargetPosition();
		}

		public int levelUpMax { get { return 1; } }

	}

}