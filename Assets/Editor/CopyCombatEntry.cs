using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Combat;
using UnityEngine.SceneManagement;

public class CopyCombatEntry {

	[MenuItem("GameObject/������������Ӧ��ս����ڲ���")]
	static void DoCopyCombatEntry(MenuCommand context) {
		GameObject target = context.context as GameObject;
		CombatRoomController room = target.GetComponentInParent<CombatRoomController>();
		if(!room) {
			Debug.LogError("������岻����Ϊһ��ս�����");
			return;
		}
		CombatEntry.clipboard[0]=SceneManager.GetActiveScene().name;
		CombatEntry.clipboard[1]=room.gameObject.name;
		CombatEntry.clipboard[2]=target.name;

	}

}
