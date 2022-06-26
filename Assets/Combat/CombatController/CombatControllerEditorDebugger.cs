using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	[ExecuteInEditMode]
	public class CombatControllerEditorDebugger:MonoBehaviour {

		PlayerData.LevelData levelData = new PlayerData.LevelData();

		CombatController _combatController;
		CombatController combatController {
			get {
				if(!_combatController) _combatController=GetComponent<CombatController>();
				return _combatController;
			}
		}

		[SerializeField] bool �Ƿ��������ع���;

		[SerializeField] CharacterParameters[] �ѷ���ɫ��Ϣ = new CharacterParameters[3];

		[SerializeField] string �ļ���;
		[SerializeField] float ��ʼλ��;
		[SerializeField] float ����λ��;
		[SerializeField] int ��ʶ���影����;
		[SerializeField] int ̼���Ͻ�����;
		[SerializeField] int ������ɫID;

		[ContextMenuItem("����ؿ�����","SaveFile")]
		[ContextMenuItem("���عؿ�����","LoadFile")]
		[SerializeField] int �Ҽ�����������������;

		string filePath { get { return $"{Application.dataPath}/StreamingAssets/{�ļ���}.xml"; } }

		void SaveFile() {

			levelData.rewardSm.value=��ʶ���影����;
			levelData.rewardPm.value=̼���Ͻ�����;
			levelData.rewardCharacter.value=������ɫID;

			levelData.startX.value=��ʼλ��;
			levelData.endX.value=����λ��;

			EntityEnemy[] enemies = GetComponentsInChildren<EntityEnemy>(true);
			for(int i = 0;i<enemies.Length;i++) {
				levelData.enemies[i].enemyType.value=enemies[i].enemyId;
				levelData.enemies[i].x.value=enemies[i].transform.position.x;
			}
			levelData.enemyCount.value=enemies.Length;

			levelData.SaveFile(filePath);
		}

		void LoadFile() {

			levelData.LoadFile(filePath);

			��ʶ���影����=levelData.rewardSm.value;
			̼���Ͻ�����=levelData.rewardPm.value;
			������ɫID=levelData.rewardCharacter.value;

			��ʼλ��=levelData.startX.value;
			����λ��=levelData.endX.value;
			combatController.DestroyAllEnemies();
			combatController.LoadAllEnemies(levelData);
		}

		private void OnDrawGizmos() {
			Vector3 pos1 = new Vector3(��ʼλ��,-100,0);
			Vector3 pos2 = new Vector3(��ʼλ��,100,0);
			Vector3 pos3 = new Vector3(����λ��,-100,0);
			Vector3 pos4 = new Vector3(����λ��,100,0);
			Gizmos.color=Color.green;
			Gizmos.DrawLine(pos1,pos2);
			Gizmos.color=Color.red;
			Gizmos.DrawLine(pos3,pos4);
		}

		private void Start() {
			if(!�Ƿ��������ع���) return;
#if UNITY_EDITOR
#else
			return;
#endif

			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=�ѷ���ɫ��Ϣ[i];
			SaveFile();
			CombatController.levelData.LoadFile(filePath);

		}

	}
}