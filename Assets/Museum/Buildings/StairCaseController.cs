using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class StairCaseController:BuildingControllerBase {

		public static bool[] floorUnlocked = { true,false,false };

		[SerializeField] BuildingControllerBase[] buildingsToUnlock;

		protected override void LevelUpFinish() {
			foreach(var i in buildingsToUnlock){
				int targetId = i.id;
				
			}
		}


	}

}