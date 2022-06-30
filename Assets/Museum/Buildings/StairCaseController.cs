using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class StairCaseController:BuildingControllerBase {

		public static bool[] floorUnlocked = { true,false,false };

		[SerializeField] int floorIndex;

		protected override void FixedUpdate() {
			base.FixedUpdate();
			if(saveData.level.value>0) floorUnlocked[floorIndex]=true;
		}

		protected override void LevelUpFinish() {
			base.LevelUpFinish();
			GameObject[] buildingsToUnlock = GameObject.FindGameObjectsWithTag($"BuildingOfFloor{floorIndex}");

			foreach(var i in buildingsToUnlock) {
				int targetId = i.GetComponent<BuildingControllerBase>().id;
				if(PlayerData.PlayerDataRoot.instance.buildingDatas[targetId].level.value<0)
					PlayerData.PlayerDataRoot.instance.buildingDatas[targetId].level.value=0;
			}
		}


	}

}