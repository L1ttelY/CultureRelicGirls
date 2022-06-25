using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaveController:MonoBehaviour {

	public static PlayerDataSaveController instance;

	private void Start() {
		if(instance) Destroy(gameObject);
		instance=this;
		DontDestroyOnLoad(gameObject);
	}

	float timeSinceSaved;
	private void Update() {
		timeSinceSaved+=Time.deltaTime;
		if(timeSinceSaved>60) PlayerData.PlayerDataController.SaveGame();
	}

	private void OnApplicationQuit() {
		PlayerData.PlayerDataController.SaveGame();
	}

}