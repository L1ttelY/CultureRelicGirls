using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	[AddComponentMenu("�����/����������")]
	[RequireComponent(typeof(CameraFocus))]
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	public class BuildingControllerBase:MonoBehaviour {

		public static string messageBuffer;

		protected SpriteRenderer spriteRenderer;
		static public Dictionary<int,BuildingControllerBase> instances = new Dictionary<int,BuildingControllerBase>();
		CountDownController.CountDownToken countDownToken;
		new Collider2D collider;

		[field: SerializeField] public int id { get; private set; }
		[field: SerializeField] public BuildingData buildingData { get; private set; }
		protected Material normalMaterial;
		[SerializeField] protected Material highlightMaterial;

		public PlayerData.BuildingData saveData { get; private set; }

		protected virtual void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
			normalMaterial=spriteRenderer.material;
			collider=GetComponent<Collider2D>();
			saveData=PlayerData.PlayerDataRoot.instance.buildingDatas[id];
			if(!instances.ContainsKey(id)) instances.Add(id,this);
			else if(instances[id]) Debug.LogError($"Duplicate @ id : {id}");
			else instances[id]=this;
		}

		protected virtual void FixedUpdate() {

			int levelUpStatus = saveData.levelUpStatus.value;

			if(levelUpStatus!=0) {
				if(countDownToken==null) countDownToken=CountDownController.instance.CreateCountDown();
				Vector3 countDownPosition = new Vector3(collider.bounds.center.x,collider.bounds.max.y)+Vector3.up*0.1f;
				countDownToken.boundObject.transform.position=countDownPosition;
				countDownToken.textField.text=saveData.levelUpProgression.TimeLeftText();
				countDownToken.progressionImage.fillAmount=saveData.levelUpProgression.progressionAmount;
			} else {
				if(countDownToken!=null) {
					CountDownController.instance.FreeCountDown(countDownToken);
					countDownToken=null;
				}
			}

			//�����������
			if(levelUpStatus!=0&&saveData.levelUpProgression.completion) {
				//�������
				spriteRenderer.material=highlightMaterial;
				saveData.levelUpStatus.value=0;
				saveData.level.value++;
				LevelUpFinish();
			}

		}

		protected virtual void LevelUpFinish() {

		}

		public virtual bool CanLevelUp() {

			//�ж��Ƿ�����
			int currentLevel = saveData.level.value;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			if(saveData.level.value>=buildingData.maxLevel) { messageBuffer="�����ȼ�����"; return false; }
			if(playerData.printingMaterial.value<buildingData.levels[currentLevel].levelUpCostMaterial) { messageBuffer="̼���ϲ���"; return false; }
			if(levelUpCount>=BuilderController.instance.levelUpMax) { messageBuffer="û�п��õ�3D��ӡ��"; return false; }
			if(saveData.levelUpStatus.value!=0) { messageBuffer="����������"; return false; }
			return true;

		}

		public virtual bool LevelUp() {
			if(!CanLevelUp()) return false;

			//��������
			int currentLevel = saveData.level.value;
			PlayerData.PlayerDataRoot playerData = PlayerData.PlayerDataRoot.instance;
			saveData.levelUpStatus.value=1;
			saveData.levelUpProgression.SetProgression(buildingData.levels[currentLevel].levelUpTime,0);
			playerData.printingMaterial.value-=buildingData.levels[currentLevel].levelUpCostMaterial;

			return true;
		}

		public virtual void OnClick(CameraFocus.CancelFocus cancelFocus) {

			BuildingLevelUpMode.EnterMode(id,null);
			spriteRenderer.material=normalMaterial;

		}

		public static int levelUpCount {
			get {
				int result = 0;
				foreach(var i in PlayerData.PlayerDataRoot.instance.buildingDatas) {
					if(i.levelUpStatus.value!=0) result++;
				}
				return result;
			}
		}

	}

}