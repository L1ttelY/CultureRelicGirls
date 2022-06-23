using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingLevelDisplay:MonoBehaviour {

	float oneSize;
	[HideInInspector] public int level;
	Image image;
	RectTransform rectTransform;

	private void Start() {
		rectTransform=transform as RectTransform;
		image=GetComponent<Image>();
		oneSize=(rectTransform.offsetMax-rectTransform.offsetMin).x;
	}

	private void Update() {

		float size = level*oneSize;
		Vector2 pos = rectTransform.offsetMax;
		pos.x=rectTransform.offsetMin.x+size;
		rectTransform.offsetMax=pos;

	}

}
