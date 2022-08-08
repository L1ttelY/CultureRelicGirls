using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public partial class CombatController:MonoBehaviour {

		public AudioClip[] walkSounds;

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
				if(friendlyList[i].characterData==null||friendlyList[i].characterData.combatPrefab==null) continue;
				CharacterParameters param = friendlyList[i];
				EntityFriendly newFriendly = Instantiate(param.characterData.combatPrefab,transform).GetComponent<EntityFriendly>();
				friendlyList[i].instance=newFriendly;
				newFriendly.transform.position=new Vector3(levelData.startX.value+(3f-i),0,0);

				int useLevel = param.use.level;
				CharacterLevelData characterLevelData = param.characterData.levels[useLevel];
				newFriendly.InitStats(characterLevelData.hpMax,characterLevelData.power,i);
			}
		}

		[HideInInspector] public int rewardSm;
		[HideInInspector] public int rewardPm;
		[SerializeField] AudioClip soundVictory;
		[SerializeField] AudioClip soundFail;

		private void FixedUpdate() {
			UpdateEndGame();
			UpdateActivateEnemy();
		}

		public bool forceEnd;

		int ticks;
		bool gameEnd;
		float endTime;
		bool endStart;
		const float timeToEnd = 1;
		void UpdateEndGame() {
			if(ticks<10) {
				ticks++;
				return;
			}

			if(gameEnd) return;
			if(endStart) endTime+=Time.deltaTime;

			if(forceEnd||!GetComponentInChildren<EntityFriendly>(true)) {
				//Ê§°Ü
				endStart=true;

				if(endTime>timeToEnd) {
					AudioController.PlayAudio(soundFail,Camera.main.transform.position);
					gameEnd=true;
					PlayerData.PlayerDataRoot.smCount+=rewardSm;
					CombatRewardUIController.instance.EnterMode(false,rewardSm,0,-1);

					foreach(var i in friendlyList) {
						int id = i.id;
						if(i.characterData&&i.characterData.combatPrefab!=null&&id>-1) {

							if(i.instance) {
								float hpAmount = (float)i.instance.hp/(float)i.instance.maxHp;
								PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=hpAmount;
							} else PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=0;
						}
					}

				}

			} else if(!GetComponentInChildren<EntityEnemy>(true)) {
				//Ê¤Àû
				endStart=true;

				if(endTime>timeToEnd) {
					AudioController.PlayAudio(soundVictory,Camera.main.transform.position);

					gameEnd=true;
					rewardSm+=levelData.rewardSm.value;
					rewardPm+=levelData.rewardPm.value;
					PlayerData.PlayerDataRoot.smCount+=rewardSm;
					PlayerData.PlayerDataRoot.pmCount+=rewardPm;

					if(levelId>=0) {
						var targetData = PlayerData.PlayerDataRoot.instance.storyStatus[levelId+2];
						if(targetData.value<1) targetData.value=1;
						targetData=PlayerData.PlayerDataRoot.instance.storyStatus[1];
						if(targetData.value<1) targetData.value=1;
					}

					int newCampaignProgression = levelId+1;
					if(PlayerData.PlayerDataRoot.instance.campaignProgression.value<newCampaignProgression)
						PlayerData.PlayerDataRoot.instance.campaignProgression.value=newCampaignProgression;

					bool characterNew = false;
					int rewardCharacterId = levelData.rewardCharacter.value;

					if(rewardCharacterId>0&&PlayerData.PlayerDataRoot.instance.characterDatas[rewardCharacterId].level.value==-1) {
						PlayerData.PlayerDataRoot.instance.characterDatas[rewardCharacterId].level.value=0;
						characterNew=true;
					}
					foreach(var i in friendlyList) {
						int id = i.id;
						if(i.characterData&&i.characterData.combatPrefab!=null&&id>-1) {

							if(i.instance) {
								float hpAmount = (float)i.instance.hp/(float)i.instance.maxHp;
								PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=hpAmount;
							} else PlayerData.PlayerDataRoot.instance.characterDatas[id].healthAmount=0;
						}
					}
					CombatRewardUIController.instance.EnterMode(true,rewardSm,rewardPm,characterNew ? levelData.rewardCharacter.value : -1);
				}

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