using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeal:ItemBase {
	public override void Use() {
		base.Use();
		Debug.Log("adsadawda");
		foreach(var i in Combat.EntityFriendly.friendlyList) {
			i.Heal(1000);
		}
	}
}
