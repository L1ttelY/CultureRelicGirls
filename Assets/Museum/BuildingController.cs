using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuildingController:MonoBehaviour {

		[field: SerializeField] public int id { get; private set; }
		[field: SerializeField] public BuildingData buildingData { get; private set; }
		public PlayerData.BuildingData saveData { get; private set; }

		private void Start() {
			saveData=PlayerData.PlayerDataRoot.instance.buildingDatas[id];
		}

		private void FixedUpdate() {

			//�����������
			if(saveData.levelUpStatus.value!=0&&saveData.levelUpProgression.completion) {
				//�������
				saveData.levelUpStatus.value=0;
				saveData.level.value++;
			}

		}

		public bool LevelUp() {

			//�ж��Ƿ�����
			int currentLevel = saveData.level.value;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			if(saveData.level.value>=buildingData.maxLevel) return false;
			if(playerData.printingMaterial.value<buildingData.levels[currentLevel].levelUpCostMaterial) return false;
			if(playerData.sentienceMatter.value<buildingData.levels[currentLevel].levelUpCostSentienceMatter) return false;
			if(levelUpCount>=MenderController.instance.levelUpMax) return false;

			//��������
			saveData.levelUpStatus.value=1;
			saveData.levelUpProgression.SetProgression(buildingData.levels[currentLevel].levelUpTime,0);
			playerData.sentienceMatter.value-=buildingData.levels[currentLevel].levelUpCostSentienceMatter;
			playerData.printingMaterial.value-=buildingData.levels[currentLevel].levelUpCostMaterial;

			return true;
		}



		public static int levelUpCount {
			get {
				int result = 0;
				foreach(var i in PlayerData.PlayerDataRoot.instance.buildingDatas) {
					if(i.levelUpStatus.value!=0) result++;
				}
				return result;
			}
		}

	}

}