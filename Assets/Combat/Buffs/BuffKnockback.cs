using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class BuffKnockback:BuffBase {

		const float speed = 10;

		public override void Update() {

			float deltaX = Mathf.Sign(stacks)*Time.deltaTime*speed;
			if(stacks>0.5f) deltaX*=(stacks/0.5f);

			owner.transform.position+=Vector3.right*deltaX;
			float newStacks = stacks-deltaX;
			if(newStacks*stacks<=0) RemoveSelf();
			stacks=newStacks;
		}

	}

}
