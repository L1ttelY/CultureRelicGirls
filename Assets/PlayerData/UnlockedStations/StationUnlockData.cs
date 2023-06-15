using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PlayerData {

	public class StationUnlockData:DataBase {


		public StationUnlockData(string name,DataBase parent) : base(name,parent) {

			foreach(var i in children) Debug.Log(i.Key);

			foreach(var i in StationData.instances) {
				Debug.Log($"{i.name} , {i.regionName} , {i.stationName}");
				DataBool data = new DataBool(i.name,this);
				unlockedStatus.Add(i,data);

			}
		}

		public static StationUnlockData instance;
		public Dictionary<StationData,DataBool> unlockedStatus = new Dictionary<StationData,DataBool>();

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