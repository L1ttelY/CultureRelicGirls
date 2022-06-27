using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class MapSelectorController:MonoBehaviour {

		[SerializeField] int mapId;
		[SerializeField] int startLevelId;
		[SerializeField] Image whiteFrame;
		[SerializeField] Image greenFrame;

		LevelSelectionMode owner;

		private void Start() {
			owner=GetComponentInParent<LevelSelectionMode>();
		}

		private void Update() {
			whiteFrame.gameObject.SetActive(PlayerData.PlayerDataRoot.instance.campaignProgression.value>=startLevelId);
			greenFrame.gameObject.SetActive(owner.selectedMap==mapId);
		}

		public void OnClick(){
			owner.OnSelectMap(mapId);
		}

	}

}