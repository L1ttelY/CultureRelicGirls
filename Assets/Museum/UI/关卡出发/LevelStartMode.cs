using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class LevelStartMode:UIModeBase {
		[SerializeField] GameObject goButton;

		[SerializeField] Text infoText;
		int levelId;

		public static LevelStartMode instance { get; private set; }

		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
		}

		public int[] chosenCharacters = new int[3];
		string targetLevelName;
		PlayerData.LevelData levelData = new PlayerData.LevelData();
		bool loadLevelFromSA;

		public static void EnterMode(bool loadLevelFromSA,string targetLevelName,int levelId) {
			instance._EnterMode(loadLevelFromSA,targetLevelName,levelId);
		}
		void _EnterMode(bool loadLevelFromSA,string targetLevelName,int levelId) {
			this.loadLevelFromSA=loadLevelFromSA;
			for(int i = 0;i<3;i++) chosenCharacters[i]=-1;
			this.targetLevelName=targetLevelName;
			levelData.LoadFile(loadLevelFromSA,targetLevelName);
			infoText.text=
				$"{levelData.levelName.value}\n"+
				$"{levelData.enemyCount.value}¸öµÐÈË";
			UIController.instance.SwitchUIMode(this);
			this.levelId=levelId;
		}

		int lastClickIndex;
		public void OnSlotClick(int index) {
			lastClickIndex=index;
			CharacterSelectionMode.EnterMode(CharacterFilter,false,CharacterSelectionCallback);
		}
		bool CharacterFilter(int id) {
			var saveData = PlayerData.PlayerDataRoot.instance.characterDatas[id];

			if(saveData.level.value<1) return false;
			if(saveData.healStatus.value!=0) return false;
			if(saveData.healthAmount==0) return false;
			for(int i = 0;i<3;i++) {
				if(chosenCharacters[i]==id) return false;
			}
			return true;
		}

		public void OnStartClick() {
			bool hasCharacter = false;
			foreach(var item in chosenCharacters) {
				if(item!=-1) hasCharacter=true;
			}
			if(!hasCharacter) return;
			Combat.CombatController.StartCombat(loadLevelFromSA,chosenCharacters,targetLevelName,levelId,new CharmData[0]);
		}

		void CharacterSelectionCallback(int characterId) {
			chosenCharacters[lastClickIndex]=characterId;
			UIController.instance.SwitchUIMode(this);
		}

		private void Update() {
			bool hasCharacter = false;
			foreach(var item in chosenCharacters) {
				if(item!=-1) hasCharacter=true;
			}
			goButton.SetActive(hasCharacter);
		}

	}

}