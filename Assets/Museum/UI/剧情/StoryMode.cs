using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using UnityEngine.UI;

namespace Museum {

	public class StoryMode:UIModeBase {

		public static StoryMode instance { get; private set; }

		[SerializeField] StoryData[] stories;
		[SerializeField] Image buttonUp;
		[SerializeField] Image buttonDown;
		[SerializeField] Text textBox;
		[SerializeField] Transform positionTitle;
		[SerializeField] Transform positionTextBox;
		[SerializeField] Transform movable;
		[SerializeField] GameObject redDotDown;
		[SerializeField] GameObject redDotUp;

		[HideInInspector] public List<StoryData> activeStories = new List<StoryData>();
		[HideInInspector] public List<int> activeIds = new List<int>();
		public override void Init() {
			base.Init();
			instance=this;
			titles=GetComponentsInChildren<StoryModeTitleController>(true);
			titleCount=titles.Length;
			for(int i = 0;i<titleCount;i++) titles[i].index=i;
			currentPage=0;
		}

		public override bool OverrideQuitButton() {
			if(isViewing) {
				isViewing=false;
				return true;
			} else return false;
		}

		public bool isViewing { get; private set; }
		public int currentPage { get; private set; }
		public StoryModeTitleController[] titles;
		[HideInInspector] public int titleCount { get; private set; }
		int pageTotal;

		public void PageChange(int amount) {
			currentPage+=amount;
			if(currentPage<0) currentPage=0;
			if(currentPage>=pageTotal) currentPage=pageTotal-1;
		}

		public static void EnterMode() => instance._EnterMode();
		void _EnterMode() {

			isViewing=false;

			activeStories.Clear();
			activeIds.Clear();
			for(int i = 0;i<stories.Length;i++) {
				if(PlayerDataRoot.instance.storyStatus[i].value>0) {
					activeIds.Add(i);
					activeStories.Add(stories[i]);
				}
			}

			pageTotal=(activeStories.Count+titleCount-1)/titleCount;
			if(pageTotal==0) pageTotal=1;

			UIController.instance.SwitchUIMode(this);
		}

		private void Update() {
			Color colorGray = new Color(.4f,.4f,.4f,.4f);
			if(currentPage==0) buttonUp.color=colorGray;
			else buttonUp.color=Color.white;
			if(currentPage>=pageTotal-1) buttonDown.color=colorGray;
			else buttonDown.color=Color.white;

			Vector3 targetPosition = Vector3.zero;
			if(isViewing) targetPosition=positionTextBox.position;
			else targetPosition=positionTitle.position;

			Vector3 velocity = targetPosition-movable.position;
			velocity*=10;
			movable.position+=velocity*Time.deltaTime;

			bool downNew = false;
			bool upNew = false;

			for(int i = 0;i<activeIds.Count;i++) {
				if(PlayerDataRoot.instance.storyStatus[activeIds[i]].value>=2) continue;
				if(i<titles[0].targetIndex) upNew=true;
				if(i>titles[titleCount-1].targetIndex) downNew=true;
			}

			redDotUp.SetActive(upNew);
			redDotDown.SetActive(downNew);

		}

		public void OnTitleClick(StoryModeTitleController sender) {
			if(isViewing) return;
			if(sender.targetIndex>=activeStories.Count) return;
			isViewing=true;
			DataInt currentData = PlayerDataRoot.instance.storyStatus[sender.targetIndex];
			if(currentData.value<2) currentData.value=2;
			textBox.text=activeStories[sender.targetIndex].text;
		}

	}

}