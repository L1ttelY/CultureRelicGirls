using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VehicleScene {

	public class TransportStationButtonController:MonoBehaviour {

		[SerializeField] GameObject isCurrentObject;
		[SerializeField] GameObject isSelectedObject;
		Text text;

		public int index { get; private set; }
		public Dictionary<string,string> regionToStation { get; private set; }
		public string targetStation => regionToStation.ContainsKey(currentRegion) ? regionToStation[currentRegion] : "";
		string currentRegion;
		public bool isUsed { get; private set; }

		TransportUIController owner;
		public void OnClick() {
			owner.OnStationSelect(targetStation);
		}

		public void ChangeRegion(string newRegion) {
			currentRegion=newRegion;
			isUsed=(targetStation.Length!=0);
			gameObject.SetActive(isUsed);
			text.text=targetStation;

			if(Combat.StationController.lastStationVisited)
				isCurrentObject.SetActive(targetStation==Combat.StationController.lastStationVisited.stationName);
			else isCurrentObject.SetActive(false);
		}

		public void Init(int index,Dictionary<string,string> regionToStation,TransportUIController owner) {
			this.index=index;
			this.owner=owner;
			this.regionToStation=regionToStation;
			text=GetComponentInChildren<Text>();
		}

		private void Update() {
			isSelectedObject.SetActive(owner.selectedStation==targetStation);
		}
	}

}