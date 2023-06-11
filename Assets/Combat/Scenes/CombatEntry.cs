using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;

[CreateAssetMenu(menuName = "�Զ�/ս����ڲ���")]
public class CombatEntry:ScriptableObject {

	public static string[] clipboard = new string[3];

	[field: SerializeField] public string sceneName { get; private set; }
	[field: SerializeField] public string roomName { get; private set; }
	[field: SerializeField] public string startObjectName { get; private set; }

	public void SetTarget(GameObject boundObject) {
		sceneName=SceneManager.GetActiveScene().name;
		roomName=boundObject.GetComponentInParent<CombatRoomController>().gameObject.name;
		startObjectName=boundObject.gameObject.name;
	}

	[ContextMenuItem("����ճ��ս����ڲ���","PasteCombatEntry")]
	[SerializeField] int �Ҽ���ճ��ս����ڲ���;
	void PasteCombatEntry() {
		sceneName=clipboard[0];
		roomName=clipboard[1];
		startObjectName=clipboard[2];
	}

}

