using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class CharacterController:MonoBehaviour {

		static readonly Vector2 inactivePosition = new Vector2(0,-10000);

		[field: SerializeField] public int characterIndex { get; private set; }
		[field: SerializeField] public CharacterData staticData { get; private set; }
		protected PlayerData.CharacterData saveData;
		protected PathFinder pathFinder;
		BuildingWithCharacterInteractionBase.SlotToken slotToken;
		CountDownController.CountDownToken countDownToken;
		SpriteRenderer spriteRenderer;
		protected Animator animator;

		protected virtual void Start() {
			saveData=PlayerData.PlayerDataRoot.instance.characterDatas[characterIndex];
			pathFinder=GetComponent<PathFinder>();
			UpdateStateChange();
			UpdateWorkEnd();
			pathFinder.SetTarget(GetTargetPosition());
			pathFinder.TeleportToTarget();
			spriteRenderer=GetComponentInChildren<SpriteRenderer>();
			animator=GetComponent<Animator>();
		}

		protected virtual void Update() {
			UpdateWanderPosition();
			UpdateWorkEnd();
			UpdateStateChange();
			pathFinder.SetTarget(GetTargetPosition());
			UpdateCountDown();
			UpdateAnimator();
		}

		protected virtual void UpdateAnimator() {
			animator.SetBool("isMoving",pathFinder.moving);
			bool isFloating = currentHealStatus!=0||currentLevelUpStatus!=0;
			isFloating&=pathFinder.arrived;
			animator.SetBool("isFloating",isFloating);
		}

		int previousHealStatus;
		int previousLevelUpStatus;

		int currentHealStatus {
			get => saveData.healStatus.value;
			set => saveData.healStatus.value=value;
		}
		int currentLevelUpStatus {
			get => saveData.levelUpStatus.value;
			set => saveData.levelUpStatus.value=value;
		}
		int currentLevel {
			get => saveData.level.value;
			set => saveData.level.value=value;
		}

		Vector2 wanderPosition=new Vector2(0,-100);
		float wanderTimer;
		const float wanderIntervalMin = 5f;
		const float wanderIntervalMax = 10f;
		void UpdateWanderPosition() {
			wanderTimer-=Time.deltaTime;
			if(wanderTimer<=0) {
				wanderTimer=Random.Range(wanderIntervalMin,wanderIntervalMax);
				ResetWanderPosition();
			}
		}
		void ResetWanderPosition() {

			FloorPath wanderFloor = pathFinder.currentFloor;
			if(Random.Range(0,1)<0.4f) {
				wanderFloor=FloorPath.floorPaths[Random.Range(0,3)];
			}
			wanderPosition=new Vector2(Random.Range(wanderFloor.leftX,wanderFloor.rightX),wanderFloor.y);

			if(!StairCaseController.floorUnlocked[wanderFloor.floorIndex]) ResetWanderPosition();

		}

		protected virtual Vector2 GetTargetPosition() {
			if(saveData.level.value<=0) return inactivePosition;
			Vector2 result = wanderPosition;
			if(slotToken!=null) result=slotToken.position;
			return result;
		}

		void UpdateStateChange() {

			if(previousHealStatus!=currentHealStatus) {
				switch(currentHealStatus) {
				case 0:
					WorkBenchController.instance.FreeSlot(slotToken);
					slotToken=null;
					Debug.Log(slotToken);
					break;
				case PlayerData.CharacterData.healTime:
					slotToken=WorkBenchController.instance.GetSlot();
					break;
				case PlayerData.CharacterData.healCost:
					slotToken=WorkBenchController.instance.GetStaticSlot();
					break;
				}
			}

			if(previousLevelUpStatus!=currentLevelUpStatus) {
				switch(currentLevelUpStatus) {
				case 0:
					LibraryController.instance.FreeSlot(slotToken);
					ResearchStationController.instance.FreeSlot(slotToken);
					slotToken=null;
					Debug.Log(slotToken);
					break;
				case PlayerData.CharacterData.levelUpTime:
					slotToken=LibraryController.instance.GetSlot();
					break;
				case PlayerData.CharacterData.levelUpCost:
					slotToken=ResearchStationController.instance.GetStaticSlot();
					break;
				}
			}

			previousHealStatus=currentHealStatus;
			previousLevelUpStatus=currentLevelUpStatus;

		}

		void UpdateWorkEnd() {
			if(currentLevelUpStatus!=0&&saveData.levelUpProgression.completion) {
				currentLevelUpStatus=0;
				currentLevel++;
			}
			if(currentHealStatus==PlayerData.CharacterData.healTime&&saveData.healProgression.completion) StopHealTime();
			if(currentHealStatus==PlayerData.CharacterData.healCost&&pathFinder.arrived) FinishHealCost();
		}

		void UpdateCountDown() {
			bool displayCountDown = currentLevelUpStatus!=0||currentHealStatus==PlayerData.CharacterData.healTime;
			if(displayCountDown) {
				if(countDownToken==null) countDownToken=CountDownController.instance.CreateCountDown();
				countDownToken.boundObject.transform.position=spriteRenderer.transform.position+Vector3.up*0.7f;
				PlayerData.Progression targetProgression;
				if(currentHealStatus!=0) targetProgression=saveData.healProgression;
				else targetProgression=saveData.levelUpProgression;
				countDownToken.textField.text=targetProgression.TimeLeftText();
				countDownToken.progressionImage.fillAmount=targetProgression.progressionAmount;
			} else {
				if(countDownToken!=null) {
					CountDownController.instance.FreeCountDown(countDownToken);
					countDownToken=null;
				}
			}
		}

		public void OnClick() {
			CharacterShowMode.EnterMode(this);
		}

		public static string messageBuffer;

		public System.TimeSpan HealTime() {
			System.TimeSpan totalTime = new System.TimeSpan((long)(System.TimeSpan.TicksPerSecond*staticData.levels[currentLevel].hpMax*staticData.healTimPerHpInSecond));
			return totalTime*saveData.healthAmount;
		}
		public int HealCost() {
			return Mathf.CeilToInt(staticData.healCostPerHp*staticData.levels[currentLevel].hpMax);
		}

		bool InWork() {
			if(currentLevelUpStatus!=0) {
				messageBuffer="正在升级";
				return true;
			}
			if(currentHealStatus!=0) {
				messageBuffer="正在修复";
				return true;
			}
			return false;
		}
		public bool CanLevelUpTime() {
			if(InWork()) return false;
			if(currentLevel>=staticData.maxLevel) {
				messageBuffer="已经满级";
				return false;
			}
			if(!LibraryController.instance.HasSlotLeft()) {
				messageBuffer="图书馆已满或不可用";
				return false;
			}
			return true;
		}
		public bool CanLevelUpCost() {
			if(InWork()) return false;
			if(currentLevel>=staticData.maxLevel) {
				messageBuffer="已经满级";
				return false;
			}
			if(!ResearchStationController.instance.HasSlotLeft()) {
				messageBuffer="研究站已满或不可用";
				return false;
			}
			if(PlayerData.PlayerDataRoot.smCount<staticData.levels[saveData.level.value].levelUpCost) {
				messageBuffer="意识晶体不足";
				return false;
			}
			return true;
		}
		public bool CanHealTime() {
			if(InWork()) return false;
			if(saveData.healthAmount>=1) {
				messageBuffer="血量已满";
				return false;
			}
			if(!WorkBenchController.instance.HasSlotLeft()) {
				messageBuffer="维修台已满或不可用";
				return false;
			}
			return true;
		}
		public bool CanHealCost() {
			if(InWork()) return false;
			if(saveData.healthAmount>=1) {
				messageBuffer="血量已满";
				return false;
			}
			if(!WorkBenchController.instance.HasSlotLeft()) {
				messageBuffer="维修台已满或不可用";
				return false;
			}
			if(PlayerData.PlayerDataRoot.smCount<HealCost()) {
				messageBuffer="意识晶体不足";
				return false;
			}
			return true;
		}

		public bool GoLevelUpTime() {
			if(!CanLevelUpTime()) return false;
			currentLevelUpStatus=PlayerData.CharacterData.levelUpTime;
			saveData.levelUpProgression.SetProgression(staticData.levels[currentLevel].levelUpTimeTime,0);
			return true;
		}
		public bool GoLevelUpCost() {
			if(!CanLevelUpCost()) return false;
			currentLevelUpStatus=PlayerData.CharacterData.levelUpCost;
			saveData.levelUpProgression.SetProgression(staticData.levels[currentLevel].levelUpCostTime,0);
			PlayerData.PlayerDataRoot.smCount-=staticData.levels[currentLevel].levelUpCost;
			return true;

		}
		public bool GoHealTime() {
			if(!CanHealTime()) return false;
			currentHealStatus=PlayerData.CharacterData.healTime;
			System.TimeSpan healtime = HealTime();
			saveData.healProgression.SetProgression(healtime,0);
			return true;
		}
		public bool GoHealCost() {
			if(!CanHealCost()) return false;
			currentHealStatus=PlayerData.CharacterData.healCost;
			return true;
		}

		public void StopHealTime() {
			float healProgression = saveData.healProgression.progressionAmount;
			saveData.healthAmount=healProgression;
			currentHealStatus=0;
		}

		void FinishHealCost() {
			PlayerData.PlayerDataRoot.smCount-=HealCost();
			saveData.healthAmount=1;
			currentHealStatus=0;
		}

	}

}