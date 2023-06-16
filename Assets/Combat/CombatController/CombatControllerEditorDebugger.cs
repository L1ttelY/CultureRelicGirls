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

		[SerializeField] bool �Ƿ��������ع���;

		[SerializeField] CharacterParameters[] �ѷ���ɫ��Ϣ = new CharacterParameters[3];

		[SerializeField] string ��ʼ����;
		[SerializeField] float ��ʼλ��;

		static bool doWork = true;

		private void Start() {

#if UNITY_EDITOR
			if(!�Ƿ��������ع���) return;
			if(!doWork) return;
			if(LoadoutController.loadoutRoot!=null) {
				foreach(var i in LoadoutController.teamMembers)
					if(i.value.Length!=0) return;
			}
			doWork=false;


			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=�ѷ���ɫ��Ϣ[i];
			CombatController.startRoom=��ʼ����;
#endif

		}

	}
}