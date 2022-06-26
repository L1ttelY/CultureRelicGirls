using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public partial class CombatController:MonoBehaviour {

		public GameObject[] enemyPrefabs;

		private void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			DestroyAllEntities();
			LoadAllEnemies(levelData);
			LoadAllFriendlies();
		}

		public void DestroyAllEntities() {
			EntityBase[] enemies = GetComponentsInChildren<EntityBase>(true);
			foreach(var i in enemies) DestroyImmediate(i.gameObject);
		}
		public void LoadAllEnemies(PlayerData.LevelData levelData) {

			int enemyCount = levelData.enemyCount.value;

			for(int i = 0;i<enemyCount;i++) {
				PlayerData.EnemyData enemy = levelData.enemies[i];
				GameObject newEnemy = Instantiate(enemyPrefabs[enemy.enemyType.value],transform);
				Vector3 position = new Vector3(enemy.x.value,0,0);
				newEnemy.transform.position=position;
			}

		}
		public void LoadAllFriendlies() {
			for(int i = 0;i<3;i++) {
				if(friendlyList[i].prefab==null) continue;
				CharacterParameters param = friendlyList[i];
				EntityFriendly newFriendly = Instantiate(param.prefab,transform).GetComponent<EntityFriendly>();
				newFriendly.transform.position=new Vector3(levelData.startX.value+(3f-i),0,0);
				newFriendly.InitStats(param.hp,param.power,i);

			}
		}

		public int rewardSm;
		public int rewardPm;

		private void Update() {
			UpdateEndGame();
		}

		int ticks;
		bool gameEnd = false;
		void UpdateEndGame() {
			if(ticks<10) {
				ticks++;
				return;
			}

			if(gameEnd) return;

			if(!GetComponentInChildren<EntityFriendly>(true)) {
				//Ê§°Ü
				gameEnd=true;
				PlayerData.PlayerDataRoot.smCount+=rewardSm;
				CombatRewardUIController.instance.EnterMode(false,rewardSm,0,-1);
			} else if(!GetComponentInChildren<EntityEnemy>(true)) {
				//Ê¤Àû
				gameEnd=true;
				rewardSm+=levelData.rewardSm.value;
				rewardPm+=levelData.rewardPm.value;
				PlayerData.PlayerDataRoot.smCount+=rewardSm;
				PlayerData.PlayerDataRoot.pmCount+=rewardPm;
				if(levelData.rewardCharacter.value>0&&PlayerData.PlayerDataRoot.instance.characterDatas[rewardSm].level.value==-1) {
					PlayerData.PlayerDataRoot.instance.characterDatas[rewardSm].level.value=0;
				}
				CombatRewardUIController.instance.EnterMode(true,rewardSm,rewardPm,levelData.rewardCharacter.value);
			}

		}

	}
}