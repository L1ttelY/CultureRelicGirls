using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VehicleScene {

	public class TransportRegionButtonController:MonoBehaviour {

		[SerializeField] GameObject isCurrentObject;
		[SerializeField] GameObject isSelectedObject;
		Text text;

		public int index { get; private set; }
		public string targetRegion { get; private set; }
		public bool isUsed { get; private set; }
		TransportUIController owner;
		public void OnClick() {
			owner.OnRegionSelect(targetRegion);
		}

		public void Init(int index,string targetRegion,TransportUIController owner) {
			this.index=index;
			this.targetRegion=targetRegion;
			this.owner=owner;
			if(targetRegion.Length==0) {
				isUsed=false;
				gameObject.SetActive(false);
			} else {
				isUsed=true;
			}

			if(Combat.StationController.lastStationVisited.Length!=0)
				isCurrentObject.SetActive(targetRegion==Combat.StationController.lastStationVisitedData.regionName);
			else isCurrentObject.SetActive(false);

			text=GetComponentInChildren<Text>();
			text.text=targetRegion;

		}

		private void Update() {
			isSelectedObject.SetActive(owner.selectedRegion==targetRegion);
		}

	}

}
