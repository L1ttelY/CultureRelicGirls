using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class CharacterSelectionMode:UIModeBase {

		public struct CharacterFilter {
			public bool usePicture;
			public int minLevel;
			public int maxLevel;
			public bool cantBeMaxLevel;
			public bool cantBeHealing;
			public bool cantBeUpgrading;
		}

		public static CharacterSelectionMode instance { get; private set; }
		public override void Init() {
			base.Init();
			if(instance) Debug.LogError("Duplicate");
			instance=this;
			slots=GetComponentsInChildren<CharacterSelectionSlot>(true);
			for(int i = 0;i<slots.Length;i++) {
				CharacterSelectionSlot slot = slots[i];
				slot.Init(i);
			}
			characters=new int[slots.Length];
		}

		public CharacterFilter filter { get; private set; }

		CharacterSelectionSlot[] slots;
		public int[] characters;

		public static void EnterMode(CharacterFilter filter) => instance._EnterMode(filter);
		void _EnterMode(CharacterFilter filter) {
			this.filter=filter;
			for(int i = 0;i<characters.Length;i++) characters[i]=-1;

			int cnt = 0;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			for(int i = 0;i<playerData.characterDatas.Length;i++) {
				if(!CharacterData.datas.ContainsKey(i)) continue;
				if(playerData.characterDatas[i].level.value<filter.minLevel) continue;
				if(playerData.characterDatas[i].level.value>filter.maxLevel) continue;
				if(filter.cantBeMaxLevel&&playerData.characterDatas[i].level.value>=CharacterData.datas[i].maxLevel) continue;
				if(filter.cantBeHealing&&playerData.characterDatas[i].healStatus.value!=0) continue;
				if(filter.cantBeUpgrading&&playerData.characterDatas[i].levelUpStatus.value!=0) continue;

				characters[cnt]=i;
				cnt++;

			}

			UIController.instance.SwitchUIMode(this);
		}

		public void Choose(int id) {
			
		}

	}
}