using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadedListener:MonoBehaviour {
	[SerializeField] GameObject root;
	void Update() {
		if(PlayerData.PlayerDataController.loaded) root.SetActive(true);
	}
}
