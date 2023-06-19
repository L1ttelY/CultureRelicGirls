using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PlayerData {

	public class CharacterDataRoot:DataBase {

		public static CharacterDataRoot instance;
		public Dictionary<string,CharacterData> characters = new Dictionary<string,CharacterData>();

		public CharacterDataRoot(string name,DataBase parent) : base(name,parent) {

			global::CharacterData.ClearInstances();
			FileManager.LoadSAAB("characterdata.ab").LoadAllAssets();

			foreach(var i in global::CharacterData.datas) {
				characters[i.Value.name]=new CharacterData(i.Key,this);
			}
		}

		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			PlayerDataRoot.OnRootCreation+=PlayerDataRoot_OnRootCreation;
		}

		private static void PlayerDataRoot_OnRootCreation(object sender) {
			PlayerDataRoot root = sender as PlayerDataRoot;
			instance=new CharacterDataRoot("CharacterDataRoot",root);
		}

	}

}
