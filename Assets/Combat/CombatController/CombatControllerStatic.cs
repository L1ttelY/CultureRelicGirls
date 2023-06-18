using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combat {

	[System.Serializable]
	//描述角色出场时的状态
	public struct CharacterUseModel {
		public int level;
		public int actionSkillType;
	}

	[System.Serializable]
	public class CharacterParameters {
		public CharacterData characterData;
		public string name => characterData.name;
		public CharacterUseModel use;

		[HideInInspector] public EntityFriendly instance;
	}

	public partial class CombatController:MonoBehaviour {

		public static string startRoom;
		public static string sceneName;
		public static CharacterParameters[] friendlyList = new CharacterParameters[3];
		public static int levelId { get; private set; }
		public static CharmData[] charmDatas = new CharmData[0];
		public static string startObject = "";

		public static void StartCombat(string sceneName,string startRoom,string startObject = "") {


			CombatController.startRoom=startRoom;
			CombatController.sceneName=sceneName;
			CombatController.startObject=startObject;

			SceneManager.LoadScene(sceneName);

		}

		public static void StartCombat(CombatEntry entry) {
			StartCombat(entry.sceneName,entry.roomName,entry.startObjectName);
		}

		public static CombatController instance { get; private set; }

		public static bool inCombat => instance;

	}
}