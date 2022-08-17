using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "×Ô¶¨/·ûÎÄ")]
public class CharmData:ScriptableObject {

	[SerializeField] string typeName;
	System.Type workingType;

	private void OnEnable() {
		workingType=System.Type.GetType("Combat."+typeName);
	}

	public Combat.CharmBase CreateCharm() {
		System.Reflection.ConstructorInfo constructor = workingType.GetConstructor(System.Type.EmptyTypes);

		if(constructor==null) return null;

		return constructor.Invoke(new object[0]) as Combat.CharmBase;
	}

}