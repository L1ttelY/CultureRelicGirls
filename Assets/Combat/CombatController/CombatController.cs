using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatController:MonoBehaviour {

	
	private void Start() {
		if(instance) Debug.LogError("Duplicate");
		instance=this;
	}

}