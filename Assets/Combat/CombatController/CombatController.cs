using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public partial class CombatController:MonoBehaviour {

		public GameObject[] enemyPrefabs;
		EntityEnemy[] enemies;

		private void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			DestroyAllEntities();
			LoadAllEnemies(levelData);
			LoadAllFriendlies();
		}

		public void DestroyAllEntities() {
			EntityBase[] entities = GetComponentsInChildren<EntityBase>(true);
			foreach(var i in entities) DestroyImmediate(i.gameObject);
		}
		public void LoadAllEnemies(PlayerData.LevelData levelData) {

			int enemyCount = levelData.enemyCount.value;

			for(int i = 0;i<enemyCount;i++) {
				PlayerData.EnemyData enemy = levelData.enemies[i];
				GameObject newEnemy = Instantiate(enemyPrefabs[enemy.enemyType.value],transform);
				Vector3 position = new Vector3(enemy.x.value,0,0);
				newEnemy.transform.position=position;
				newEnemy.gameObject.SetActive(false);
			}

			enemies=GetComponentsInChildren<EntityEnemy>(true);

		}
		public void LoadAllFriendlies() {
			for(int i = 0;i<3;i++) {
				if(friendlyList[i].prefab==null) continue;
				CharacterParameters param = friendlyList[i];
				EntityFriendly newFriendly = Instantiate(param.prefab,transform).GetComponent<EntityFriendly>();
				friendlyList[i].instance=newFriendly;
				newFriendly.transform.position=new Vector3(levelData.startX.value+(3f-i),0,0);
				newFriendly.InitStats(param.hp,param.power,i,friendlyList[i].hpAmount);
			}
		}

		[HideInInspector] public int rewardSm;
		[HideInInspector] public int rewardPm;

		private void FixedUpdate() {
			UpdateEndGame();
			UpdateActivateEnemy();
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
				//ʧ��
				gameEnd=true;
				PlayerData.PlayerDataRoot.smCount+=rewardSm;
				CombatRewardUIController.instance.EnterMode(false,rewardSm,0,-1);
				foreach(var i in friendlyList) {
					int id = i.id;
					if(i.prefab!=null&&id>0) PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=0;
				}

			} else if(!GetComponentInChildren<EntityEnemy>(true)) {
				//ʤ��
				gameEnd=true;
				rewardSm+=levelData.rewardSm.value;
				rewardPm+=levelData.rewardPm.value;
				PlayerData.PlayerDataRoot.smCount+=rewardSm;
				PlayerData.PlayerDataRoot.pmCount+=rewardPm;

				int newCampaignProgression = levelId+1;
				if(PlayerData.PlayerDataRoot.instance.campaignProgression.value<newCampaignProgression)
					PlayerData.PlayerDataRoot.instance.campaignProgression.value=newCampaignProgression;

				int rewardCharacterId = levelData.rewardCharacter.value;
				if(rewardCharacterId>0&&PlayerData.PlayerDataRoot.instance.characterDatas[rewardCharacterId].level.value==-1) {
					PlayerData.PlayerDataRoot.instance.characterDatas[rewardCharacterId].level.value=0;
				}
				foreach(var i in friendlyList) {
					int id = i.id;
					if(i.prefab!=null&&id>0) {

						if(i.instance) {
							float hpAmount = (float)i.instance.hp/(float)i.instance.maxHp;
							PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=hpAmount;
						} else PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=0;
					}
				}
				CombatRewardUIController.instance.EnterMode(true,rewardSm,rewardPm,levelData.rewardCharacter.value);
			}

		}

		const float activateRange = 12;
		void UpdateActivateEnemy() {

			float rightX = float.MinValue;
			foreach(EntityFriendly i in EntityFriendly.friendlyList) {
				if(!i) continue;
				rightX=Mathf.Max(rightX,i.transform.position.x);
			}

			foreach(var i in enemies) {

				if(!i) continue;
				if(i.gameObject.activeInHierarchy) continue;
				if(i.transform.position.x>rightX+activateRange) continue;

				i.gameObject.SetActive(true);

			}
		}

	}
}