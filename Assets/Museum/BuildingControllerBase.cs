using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class BuildingControllerBase:MonoBehaviour {

		SpriteRenderer spriteRenderer;
		static public Dictionary<int,BuildingControllerBase> instances = new Dictionary<int,BuildingControllerBase>();

		[field: SerializeField] public int id { get; private set; }
		[field: SerializeField] public BuildingData buildingData { get; private set; }
		Material normalMaterial;
		[SerializeField] Material highlightMaterial;

		public PlayerData.BuildingData saveData { get; private set; }

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
			normalMaterial=spriteRenderer.material;
			saveData=PlayerData.PlayerDataRoot.instance.buildingDatas[id];
			if(!instances.ContainsKey(id)) instances.Add(id,this);
			else if(instances[id]) Debug.LogError("Duplicate");
			else instances[id]=this;
		}

		private void FixedUpdate() {

			//更新升级情况
			if(saveData.levelUpStatus.value!=0&&saveData.levelUpProgression.completion) {
				//升级完毕
				spriteRenderer.material=highlightMaterial;
				saveData.levelUpStatus.value=0;
				saveData.level.value++;
			}

		}

		public bool CanLevelUp() {

			//判断是否升级
			int currentLevel = saveData.level.value;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			if(saveData.level.value>=buildingData.maxLevel) return false;
			if(playerData.printingMaterial.value<buildingData.levels[currentLevel].levelUpCostMaterial) return false;
			if(levelUpCount>=BuilderController.instance.levelUpMax) return false;
			if(saveData.levelUpStatus.value!=0) return false;
			return true;

		}

		public bool LevelUp() {
			if(!CanLevelUp()) return false;

			//可以升级
			int currentLevel = saveData.level.value;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			saveData.levelUpStatus.value=1;
			saveData.levelUpProgression.SetProgression(buildingData.levels[currentLevel].levelUpTime,0);
			playerData.printingMaterial.value-=buildingData.levels[currentLevel].levelUpCostMaterial;

			return true;
		}

		public void OnClick(CameraFocus.CancelFocus cancelFocus) {

			BuildingLevelUpMode.EnterMode(id,null);
			spriteRenderer.material=normalMaterial;

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