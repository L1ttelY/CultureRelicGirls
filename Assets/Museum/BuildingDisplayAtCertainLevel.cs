using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

namespace Museum {
	[AddComponentMenu("博物馆/建筑物显示控制器")]
	public class BuildingDisplayAtCertainLevel:MonoBehaviour {

		[SerializeField] Sprite[] sprites;
		[SerializeField] int buildingId;

		SpriteRenderer spriteRenderer;

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		private void Update() {
			int currentLevel = PlayerDataRoot.instance.buildingDatas[buildingId].level.value;

			if(currentLevel==-1) spriteRenderer.color=Color.clear;
			else {
				spriteRenderer.color=Color.white;
				spriteRenderer.sprite=sprites[currentLevel];
			}

		}

	}
}