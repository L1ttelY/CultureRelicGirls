using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EntityEnemy:EntityBase{

		[SerializeField] protected float wakeUpDistance;

		protected override void Start() {
			base.Start();
			StartIdle();
		}

		protected virtual void StartIdle(){
			currensState=StateIdle;
		}

		protected virtual void StateIdle(){
			float x = transform.position.x;
			bool toActive=false;
			if(x<=EntityFriendly.rightestX+wakeUpDistance) toActive=true;
			if(x>=EntityFriendly.leftestX-wakeUpDistance) toActive=true;
			if(toActive) StartMove();
		}


	}

}