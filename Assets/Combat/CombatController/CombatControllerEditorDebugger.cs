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

		[SerializeField] CharmData[] ����;

		[SerializeField] string ��ʼ����;
		[SerializeField] float ��ʼλ��;

		private void Start() {
			if(!�Ƿ��������ع���) return;
#if UNITY_EDITOR
#else
			return;
#endif

			for(int i = 0;i<3;i++) CombatController.friendlyList[i]=�ѷ���ɫ��Ϣ[i];
			CombatController.charmDatas=����;
			CombatController.startRoom=��ʼ����;
			CombatController.startX=��ʼλ��;

		}

	}
}