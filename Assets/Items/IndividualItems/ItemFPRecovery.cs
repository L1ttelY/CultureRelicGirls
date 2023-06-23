using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFPRecovery:ItemBase {
	public override void Use() {
		base.Use();

		Combat.Player.instance.mana+=500;
	}
}
