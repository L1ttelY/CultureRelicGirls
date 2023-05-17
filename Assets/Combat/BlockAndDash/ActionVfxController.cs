using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class ActionVfxController:MonoBehaviour {

		Animator animator;

		[SerializeField] GameObject vfxParry;

		[SerializeField] float friendlyAlpha;

		void Start() {
			animator=GetComponent<Animator>();
		}

		private void Update() {
			animator.SetBool("isBlocking",Player.instance.isBlocking);
			animator.SetBool("isDashing",EntityFriendly.isAnyoneCharging);
			animator.SetBool("isParrying",Player.instance.isInvincible);

			float posX = 0;
			Vector3 posY= Vector3.zero;
			int friendlyCount = 0;

			foreach(var i in EntityFriendly.friendlyList) {
				if(!i) continue;
				posY=i.transform.position;
				posX+=i.targetFinalPosition;
				friendlyCount++;
			}
			posX/=friendlyCount;
			posY.x=posX;
			transform.position=posY;

			foreach(var i in EntityFriendly.friendlyList) {
				if(!i) continue;
				Color col = i.spriteRenderer.color;
				col.a=friendlyAlpha;
				i.spriteRenderer.color=col;
			}
			friendlyAlpha=1;

		}

		public void CreateParryVfx() {
			VfxPool.Create(vfxParry,transform.position,EntityFriendly.friendlyLastDamage.amount);
		}

		public void GatherForBlock(){
			foreach(var i in EntityFriendly.friendlyList){
				if(!i) continue;
				Vector3 pos = i.transform.position;
				pos.x=i.targetFinalPosition;
				i.transform.position=pos;
			}
		}

	}

}
