using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class EmptyMode:UIModeBase {

		static EmptyMode instance;
		public override void Init() {
			base.Init();
			if(instance) Debug.Log("Duplicate");
			instance=this;
		}

		public static void EnterMode() { instance._EnterMode(); }

		void _EnterMode() { UIController.instance.SwitchUIMode(this); }

	}

}