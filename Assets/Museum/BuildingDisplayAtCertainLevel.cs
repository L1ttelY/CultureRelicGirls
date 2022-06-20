using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

namespace Museum {
	[AddComponentMenu("�����/���ض��ȼ���ʾ")]
	public class BuildingDisplayAtCertainLevel:MonoBehaviour {

		[SerializeField] int targetLevel;
		[SerializeField] int buildingId;

		SpriteRenderer spriteRenderer;

		private void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		private void Update() {
			if(PlayerDataRoot.instance.buildingDatas[buildingId].level.value==targetLevel) spriteRenderer.color=Color.white;
			else spriteRenderer.color=Color.clear;

			Debug.Log(PlayerDataRoot.instance.buildingDatas[buildingId].level.value);

		}

	}
}