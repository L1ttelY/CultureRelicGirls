using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Home;

namespace VehicleScene {

	public class TransportUIController:MonoBehaviour {

		StationData targetStation;

		TransportRegionButtonController[] regions;
		TransportStationButtonController[] stations;
		public string selectedRegion = "";
		public string selectedStation = "";

		private void Start() {
			regions=GetComponentsInChildren<TransportRegionButtonController>();
			stations=GetComponentsInChildren<TransportStationButtonController>();

			List<StationData> activeStations = new List<StationData>();
			foreach(var i in StationData.instances)
				if(PlayerData.StationUnlockData.instance.unlockedStatus[i].value)
					activeStations.Add(i);
			activeStations.Sort();

			List<string> regionStrings = new List<string>();
			Dictionary<string,List<string>> stationStrings = new Dictionary<string,List<string>>();

			foreach(var i in activeStations) {
				if(!regionStrings.Contains(i.regionName)) {
					regionStrings.Add(i.regionName);
					stationStrings.Add(i.regionName,new List<string>());
				}
				stationStrings[i.regionName].Add(i.stationName);
			}

			for(int i = 0;i<regions.Length;i++) {
				regions[i].Init(i,i<regionStrings.Count ? regionStrings[i] : "",this);
			}
			for(int i = 0;i<stations.Length;i++) {
				Dictionary<string,string> regionToStation = new Dictionary<string,string>();
				foreach(var region in stationStrings) {
					if(i<region.Value.Count) regionToStation.Add(region.Key,region.Value[i]);
					else regionToStation.Add(region.Key,"");
				}
				stations[i].Init(i,regionToStation,this);
				stations[i].ChangeRegion("");
			}



		}

		public void OnRegionSelect(string region) {
			selectedRegion=region;
			selectedStation="";
			foreach(var i in stations) {
				i.ChangeRegion(region);
			}
		}

		public void OnStationSelect(string station) {
			StationData chosenStation = null;
			foreach(var i in StationData.instances) if(i.stationName==station) chosenStation=i;
			if(!chosenStation) return;
			targetStation=chosenStation;
			selectedStation=station;
		}

		public void OnTravelConfirm() {
			if(targetStation!=null)
				Combat.CombatController.StartCombat(targetStation);
		}

	}
}
