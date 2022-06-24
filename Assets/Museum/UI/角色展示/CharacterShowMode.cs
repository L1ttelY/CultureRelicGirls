using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CharacterShowMode:UIModeBase {

		[SerializeField] BuildingLevelDisplay levelDisplay;
		[SerializeField] Text description;

		static CharacterShowMode instance;
		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		int targetId;
		CharacterController targetController;

		public static void EnterMode(int id,CharacterController controller,CharacterData data){

		}
		void _EnterMode(){

		}

	}

}