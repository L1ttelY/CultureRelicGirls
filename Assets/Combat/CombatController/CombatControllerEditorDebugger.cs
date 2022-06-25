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

		[SerializeField] bool 是否启用重载功能;

		[SerializeField] CharacterParameters[] 友方角色信息 = new CharacterParameters[3];

		[SerializeField] string 文件名;
		[SerializeField] float 开始位置;
		[SerializeField] float 结束位置;
		[SerializeField] int 意识晶体奖励量;
		[SerializeField] int 碳材料奖励量;
		[SerializeField] int 奖励角色ID;

		[ContextMenuItem("保存关卡布置","SaveFile")]
		[ContextMenuItem("加载关卡布置","LoadFile")]
		[SerializeField] int 右键我来进行其他操作;

		string filePath { get { return $"{Application.dataPath}/StreamingAssets/{文件名}.xml"; } }

		void SaveFile() {

			levelData.rewardSm.value=意识晶体奖励量;
			levelData.rewardPm.value=碳材料奖励量;
			levelData.rewardCharacter.value=奖励角色ID;

			levelData.startX.value=开始位置;
			levelData.endX.value=结束位置;

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

			意识晶体奖励量=levelData.rewardSm.value;
			碳材料奖励量=levelData.rewardPm.value;
			奖励角色ID=levelData.rewardCharacter.value;

			开始位置=levelData.startX.value;
			结束位置=levelData.endX.value;
			combatController.DestroyAllEnemies();
			combatController.LoadAllEnemies(levelData);
		}

		private void OnDrawGizmos() {
			Vector3 pos1 = new Vector3(开始位置,-100,0);
			Vector3 pos2 = new Vector3(开始位置,100,0);
			Vector3 pos3 = new Vector3(结束位置,-100,0);
			Vector3 pos4 = new Vector3(结束位置,100,0);
			Gizmos.color=Color.green;
			Gizmos.DrawLine(pos1,pos2);
			Gizmos.color=Color.red;
			Gizmos.DrawLine(pos3,pos4);
		}

		private void Start() {
			if(!是否启用重载功能) return;
#if UNITY_EDITOR
#else
			return;
#endif

			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=友方角色信息[i];
			SaveFile();
			CombatController.levelData.LoadFile(filePath);

		}

	}
}