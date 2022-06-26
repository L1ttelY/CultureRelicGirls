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
	}

	public partial class CombatController:MonoBehaviour {

		public static CharacterParameters[] friendlyList = new CharacterParameters[3];
		public static float startX { get { return levelData.startX.value; } }
		public static float endX { get { return levelData.endX.value; } }
		public static PlayerData.LevelData levelData = new PlayerData.LevelData();

		public static void StartCombat(int[] friendlyIds,string levelPath) {
			for(int i = 0;i<3;i++) {
				int id = friendlyIds[i];
				CharacterData targetData = CharacterData.datas[id];
				CharacterParameters target = new CharacterParameters();
				int level = PlayerData.PlayerDataRoot.instance.characterDatas[id].level.value;
				friendlyList[i]=target;

				target.prefab=targetData.combatPrefab;
				target.hp=targetData.levels[level].hpMax;
				target.power=targetData.levels[level].power;
				target.id=id;
			}

			levelData.LoadFile(levelPath);
			string sceneName = levelData.sceneName.value;
			SceneManager.LoadScene(sceneName);

		}

		public static CombatController instance { get; private set; }

	}
}