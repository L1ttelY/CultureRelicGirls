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

		[SerializeField] string 起始房间;
		[SerializeField] float 起始位置;

		static bool doWork = true;

		private void Start() {

#if UNITY_EDITOR
			if(!是否启用重载功能) return;
			if(!doWork) return;
			if(LoadoutController.loadoutRoot!=null) {
				foreach(var i in LoadoutController.teamMembers)
					if(i.value.Length!=0) return;
			}
			doWork=false;


			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=友方角色信息[i];
			CombatController.startRoom=起始房间;
#endif

		}

	}
}