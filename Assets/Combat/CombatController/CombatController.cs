using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public partial class CombatController:MonoBehaviour {

		public GameObject[] enemyPrefabs;

		private void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			DestroyAllEnemies();
			LoadAllEnemies(levelData);

		}

		public void DestroyAllEnemies() {
			EntityEnemy[] enemies = GetComponentsInChildren<EntityEnemy>(true);
			foreach(var i in enemies) Destroy(i.gameObject);
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

		public int rewardSm;
		public int rewardPm;

	}
}