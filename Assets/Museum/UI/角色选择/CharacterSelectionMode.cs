using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class CharacterSelectionMode:UIModeBase {

		public delegate void SelectionCallback(int id);
		public delegate bool Filter(int id);

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

		public Filter filter { get; private set; }
		public bool usePicture;
		SelectionCallback callback;

		CharacterSelectionSlot[] slots;
		[HideInInspector] public int[] characters;

		public static void EnterMode(Filter filter,bool usePicture,SelectionCallback callback) => instance._EnterMode(filter,usePicture,callback);
		void _EnterMode(Filter filter,bool usePicture,SelectionCallback callback) {
			this.filter=filter;
			this.callback=callback;
			for(int i = 0;i<characters.Length;i++) characters[i]=-1;

			int cnt = 0;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			for(int i = 0;i<playerData.characterDatas.Length;i++) {
				if(!CharacterData.datas.ContainsKey(i)) continue;
				if(!filter(i)) continue;

				characters[cnt]=i;
				cnt++;

			}

			UIController.instance.SwitchUIMode(this);
		}

		public void Choose(int id) {
			if(characters[id]==-1) return;
			EmptyMode.EnterMode();
			callback?.Invoke(characters[id]);
		}

	}
}