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
		public int id => characterData.id;
		public CharacterUseModel use;

		[HideInInspector] public EntityFriendly instance;
	}

	public partial class CombatController:MonoBehaviour {

		public static float startX;
		public static string startRoom;
		public static string sceneName;
		public static CharacterParameters[] friendlyList = new CharacterParameters[3];
		public static int levelId { get; private set; }
		public static CharmData[] charmDatas;

		public static void StartCombat(CharacterParameters[] friendlyDatas,string sceneName,string startRoom,float startX,CharmData[]charms) {
			
			for(int i=0;i<3;i++){
				friendlyList[i]=friendlyDatas[i];
			}
			
			charmDatas=charms;

			CombatController.startX=startX;
			CombatController.startRoom=startRoom;
			CombatController.sceneName=sceneName;

		}

		public static CombatController instance { get; private set; }

	}
}