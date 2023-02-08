using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {
	[ExecuteInEditMode]
	[ExcludeFromPreset]
	public class CombatControllerEditorDebugger:MonoBehaviour {

		CombatController _combatController;
		CombatController combatController {
			get {
				if(!_combatController) _combatController=GetComponent<CombatController>();
				return _combatController;
			}
		}

		[SerializeField] bool 是否启用重载功能;

		[SerializeField] CharacterParameters[] 友方角色信息 = new CharacterParameters[3];

		[SerializeField] CharmData[] 符文;

		[SerializeField] string 起始房间;
		[SerializeField] float 起始位置;

		private void Start() {
			if(!是否启用重载功能) return;
#if UNITY_EDITOR
#else
			return;
#endif

			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=友方角色信息[i];
			CombatController.charmDatas=符文;
			CombatController.startRoom=起始房间;
			CombatController.startX=起始位置;

		}

	}
}