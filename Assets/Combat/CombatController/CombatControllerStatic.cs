using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	[System.Serializable]
	public class CharacterParameters {
		public GameObject prefab;
		public int hp;
		public int power;
		public int id;
		public float hpAmount;
		[HideInInspector] public EntityFriendly instance;
	}

	public partial class CombatController:MonoBehaviour {

		public static CharacterParameters[] friendlyList = new CharacterParameters[3];
		public static float startX { get { return levelData.startX.value; } }
		public static float endX { get { return levelData.endX.value; } }
		public static PlayerData.LevelData levelData = new PlayerData.LevelData();
		public static int levelId { get; private set; }

		public static void StartCombat(bool isSA,int[] friendlyIds,string levelName,int levelId) {
			for(int i = 0;i<3;i++) {
				int id = friendlyIds[i];
				if(id>=0) {
					CharacterData targetData = CharacterData.datas[id];
					CharacterParameters target = new CharacterParameters();
					var saveData = PlayerData.PlayerDataRoot.instance.characterDatas[id];
					int level = saveData.level.value;

					friendlyList[i]=target;

					target.prefab=targetData.combatPrefab;
					target.hp=targetData.levels[level].hpMax;
					target.power=targetData.levels[level].power;
					target.id=id;
					target.hpAmount=saveData.healthAmount;
					CombatController.levelId=levelId;
				} else {
					CharacterParameters target = new CharacterParameters();
					target.prefab=null;
					friendlyList[i]=target;
				}
			}

			levelData.LoadFile(isSA,levelName);
			string sceneName = levelData.sceneName.value;
			SceneManager.LoadScene(sceneName);

		}

		public static CombatController instance { get; private set; }

	}
}