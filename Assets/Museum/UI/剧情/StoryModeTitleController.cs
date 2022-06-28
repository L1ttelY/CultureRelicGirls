using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class StoryModeTitleController:MonoBehaviour {

		StoryMode owner;
		[HideInInspector] public int index;
		Text text;
		[SerializeField] GameObject redDot;

		public int targetIndex { get { return owner.currentPage*owner.titles.Length+index; } }

		private void Start() {
			owner=GetComponentInParent<StoryMode>();
			text=GetComponentInChildren<Text>();
		}

		private void Update() {

			Debug.Log($"id count : {owner.activeIds.Count} , story count : {owner.activeStories.Count}");

			if(targetIndex>=owner.activeIds.Count) text.text="";
			else text.text=owner.activeStories[targetIndex].title;

			bool isNew = false;
			if(targetIndex<owner.activeIds.Count)
				isNew=PlayerData.PlayerDataRoot.instance.storyStatus[owner.activeIds[targetIndex]].value<2;
			redDot.SetActive(isNew);

		}

		public void Onclick() {
			owner.OnTitleClick(this);
		}

	}

}