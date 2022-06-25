using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

namespace Museum {
	[AddComponentMenu("博物馆/建筑物显示控制器")]
	[ExecuteInEditMode]
	public class BuildingDisplayAtCertainLevel:MonoBehaviour {

		[SerializeField] Sprite[] sprites;
		[SerializeField] int buildingId;

		[SerializeField] int overrideLevel;

		SpriteRenderer spriteRenderer;

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		private void Update() {

			try {
				int currentLevel = PlayerDataRoot.instance.buildingDatas[buildingId].level.value;

				if(currentLevel==-1) spriteRenderer.color=Color.clear;
				else {
					spriteRenderer.color=Color.white;
					spriteRenderer.sprite=sprites[currentLevel];
				}

				if(!Application.isPlaying) throw new System.Exception();

			} catch(System.Exception e) {
				Debug.Log("override");
				if(!spriteRenderer) spriteRenderer=GetComponent<SpriteRenderer>();
				spriteRenderer.sprite=sprites[overrideLevel];
				spriteRenderer.color=Color.white;
			}

		}

	}
}