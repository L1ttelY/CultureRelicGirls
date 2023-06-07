using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PlayerData {

	public class StationUnlockData:DataBase {


		public StationUnlockData(string name,DataBase parent) : base(name,parent) {
			foreach(var i in StationData.instances) {
				DataBool data = new DataBool(i.name,this);
			}
		}

		public override void Load(XmlElement serialized) {
			base.Load(serialized);

			Debug.Log("STATIONS : ");
			foreach(var i in children) Debug.Log($"{i.Key},{(i.Value as DataBool).value}");
		}

		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			PlayerDataRoot.OnRootCreation+=PlayerDataRoot_OnRootCreation;
		}

		private static void PlayerDataRoot_OnRootCreation(object sender) {
			PlayerDataRoot root = sender as PlayerDataRoot;
			StationUnlockData data = new StationUnlockData("StationUnlock",root);
		}
	}

}