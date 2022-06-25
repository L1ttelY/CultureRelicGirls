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
		public static float startX;
		public static float startY;
		public static PlayerData.LevelData levelData=new PlayerData.LevelData();

		public static void StartCombat(int[] friendlyIds,string sceneName,string levelPath) {
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
			SceneManager.LoadScene(sceneName);

		}

		public static CombatController instance { get; private set; }

	}
}