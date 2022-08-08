using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	[System.Serializable]
	//描述角色出场时的状态
	public struct CharacterUseModel {
		public int level;
		public bool useActionSkill;
	}

	[System.Serializable]
	public class CharacterParameters {
		public CharacterData characterData;
		public int id;
		public CharacterUseModel use;

		[HideInInspector] public EntityFriendly instance;
	}

	public partial class CombatController:MonoBehaviour {

		public static CharacterParameters[] friendlyList = new CharacterParameters[3];
		public static float startX { get { return levelData.startX.value; } }
		public static float endX { get { return levelData.endX.value; } }
		public static PlayerData.LevelData levelData = new PlayerData.LevelData();
		public static int levelId { get; private set; }

		public static void StartCombat(bool isSA,int[] friendlyIds,string levelName,int levelId) {

			CombatController.levelId=levelId;

			for(int i = 0;i<3;i++) {
				int id = friendlyIds[i];
				if(id>=0) {
					CharacterData targetData = CharacterData.datas[id];
					CharacterParameters target = new CharacterParameters();
					var saveData = PlayerData.PlayerDataRoot.instance.characterDatas[id];

					friendlyList[i]=target;

					target.use.level=saveData.level.value;
					target.characterData=targetData;
					target.id=id;

				} else {
					CharacterParameters target = new CharacterParameters();
					target.characterData=null;
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