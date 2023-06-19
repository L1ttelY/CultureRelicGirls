using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PlayerData {

	public class StationUnlockData:DataBase {


		public StationUnlockData(string name,DataBase parent) : base(name,parent) {

			StationData.ClearInstances();
			var ab=FileManager.LoadSAAB("stationdata.ab"); ab.LoadAllAssets();

			foreach(var i in StationData.instances) {
				DataBool data = new DataBool(i.Value.name,this);
				unlockedStatus.Add(i.Value.name,data);

			}
		}

		public static StationUnlockData instance;
		public Dictionary<string,DataBool> unlockedStatus = new Dictionary<string,DataBool>();

		public override void Load(XmlElement serialized) {
			base.Load(serialized);

		}

		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			PlayerDataRoot.OnRootCreation+=PlayerDataRoot_OnRootCreation;
		}

		private static void PlayerDataRoot_OnRootCreation(object sender) {
			PlayerDataRoot root = sender as PlayerDataRoot;
			StationUnlockData data = new StationUnlockData("StationUnlock",root);
			instance=data;
		}
	}

}