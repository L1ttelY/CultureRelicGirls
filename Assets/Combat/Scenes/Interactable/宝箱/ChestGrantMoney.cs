using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

  public class ChestGrantMoney:MonoBehaviour {

    [SerializeField] int smCount;

    public void OnInteract(){
      PlayerData.PlayerDataRoot.smCount+=smCount;
		}

  }

}