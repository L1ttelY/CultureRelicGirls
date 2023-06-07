using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Combat;

[CreateAssetMenu(menuName = "自定/站台信息")]
public class StationData:ScriptableObject {
	[SerializeField] string sceneName;
	[SerializeField] string roomName;
	[SerializeField] string startObjectName;

	[SerializeField] string regionName;
	[SerializeField] string stationName;

	public static void ClearInstances() {
		instances.Clear();
	}

	public static readonly HashSet<StationData> instances = new HashSet<StationData>();

	public void SetTarget(GameObject boundObject) {
		sceneName=SceneManager.GetActiveScene().name;
		roomName=boundObject.GetComponentInParent<CombatRoomController>().gameObject.name;
		startObjectName=boundObject.gameObject.name;
	}

	private void OnEnable() {
		instances.Add(this);
	}
}
