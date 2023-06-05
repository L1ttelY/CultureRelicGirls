using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationData:ScriptableObject {
	[SerializeField] string sceneName;
	[SerializeField] string roomName;
	[SerializeField] string startObjectName;

	[SerializeField] string regionName;
	[SerializeField] string stationName;
}
