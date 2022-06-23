using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

namespace Museum {
	[AddComponentMenu("�����/��������ʾ������")]
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