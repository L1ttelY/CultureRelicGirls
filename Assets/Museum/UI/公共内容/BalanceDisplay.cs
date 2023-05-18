using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

  public class BalanceDisplay:MonoBehaviour {
    [SerializeField] Text smText;
    [SerializeField] Text pmText;

		private void Update() {
			smText.text=PlayerData.PlayerDataRoot.instance.sentienceMatter.value.ToString();
			pmText.text=PlayerData.PlayerDataRoot.instance.printingMaterial.value.ToString();
		}

	}

}