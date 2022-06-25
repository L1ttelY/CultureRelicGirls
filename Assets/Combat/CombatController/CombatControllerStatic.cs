using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatController:MonoBehaviour {
	//static
	public static GameObject friendly1 { get; private set; }
	public static GameObject friendly2 { get; private set; }
	public static GameObject friendly3 { get; private set; }

	public static void StartCombat(int friendly1,int friendly2,int friendly3,string sceneName,string levelName,bool streamingAssetLevel) {

	}

	public static CombatController instance{ get; private set; }

}