using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	public partial class CombatController:MonoBehaviour {

		public AudioClip[] walkSounds;

		CharmBase[] activeCharms;

		private void Start() {
			if(instance) Debug.LogError("Duplicate");
			instance=this;

			LoadAllFriendlies();

			activeCharms=new CharmBase[charmDatas.Length];
			for(int i = 0;i<charmDatas.Length;i++) {
				Debug.Log("I "+i);
				activeCharms[i]=charmDatas[i].CreateCharm();
			}
		}

		public void LoadAllFriendlies() {

			Transform room = GameObject.Find(startRoom).transform;
			CombatRoomController roomScript = room.GetComponentInChildren<CombatRoomController>();
			Vector3 startPosition = new Vector3(roomScript.initialX,room.position.y);

			if(room&&startObject!=null&&startObject.Length>0&&room.Find(startObject))
				startPosition.x=room.Find(startObject).position.x;
			startObject="";

			for(int i = 0;i<3;i++) {

				if(friendlyList[i].characterData==null||friendlyList[i].characterData.combatPrefab==null) continue;
				CharacterParameters param = friendlyList[i];
				EntityFriendly newFriendly = Instantiate(param.characterData.combatPrefab,transform).GetComponent<EntityFriendly>();
				newFriendly.SetUse(param.use);
				friendlyList[i].instance=newFriendly;

				newFriendly.transform.position=startPosition;
				newFriendly.transform.parent=room;

				int useLevel = param.use.level;
				CharacterLevelData characterLevelData = param.characterData.levels[useLevel];
				newFriendly.InitStats(characterLevelData,i);

			}
		}

		[HideInInspector] public int rewardSm;
		[HideInInspector] public int rewardPm;
		[SerializeField] AudioClip soundVictory;
		[SerializeField] AudioClip soundFail;

		private void FixedUpdate() {
			UpdateEndGame();
			foreach(var i in activeCharms) i.Update();
		}

		public bool forceEnd;

		int ticks;
		bool gameEnd;
		float endTime;
		bool endStart;
		const float timeToEnd = 1;
		void UpdateEndGame() {
			if(ticks<10) {
				ticks++;
				return;
			}

			if(gameEnd) return;
			if(endStart) endTime+=Time.deltaTime;

		}

		const float activateRange = 12;

		private void OnDestroy() {
			foreach(var i in activeCharms) i.Terminate();
		}

	}
}