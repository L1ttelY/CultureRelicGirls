using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDisplay:MonoBehaviour {

	[SerializeField] UnityEngine.UI.Text text;

	private void Update() {
		text.text=PlayerData.PlayerDataRoot.smCount.ToString();
	}

}

