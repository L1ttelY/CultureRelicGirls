using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Combat;
using UnityEngine.SceneManagement;

public class CopyCombatEntry {

	[MenuItem("GameObject/复制这个对象对应的战斗入口参数")]
	static void DoCopyCombatEntry(MenuCommand context) {
		GameObject target = context.context as GameObject;
		CombatRoomController room = target.GetComponentInParent<CombatRoomController>();
		if(!room) {
			Debug.LogError("这个物体不能作为一个战斗入口");
			return;
		}
		CombatEntry.clipboard[0]=SceneManager.GetActiveScene().name;
		CombatEntry.clipboard[1]=room.gameObject.name;
		CombatEntry.clipboard[2]=target.name;

	}

}
