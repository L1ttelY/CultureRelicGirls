using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class ActionButtonController:MonoBehaviour {

		[SerializeField] GameObject iconDash;
		[SerializeField] GameObject iconBlock;
		[SerializeField] Image dashCd;

		[SerializeField] GameObject dashAvailable;
		[SerializeField] GameObject dashing;
		[SerializeField] GameObject blockAvailable;
		[SerializeField] GameObject blocking;

		void Start() {

		}

		float angle = 180;

		void Update() {

			float deltaAngle = 1000*Time.deltaTime;
			if(Player.instance.toDash) {
				if(angle>deltaAngle) angle-=deltaAngle;
				else angle=0;
			} else {
				if(angle<180-deltaAngle) angle+=deltaAngle;
				else angle=180;
			}

			Quaternion rotation = Quaternion.Euler(angle,0,0);

			transform.rotation=rotation;

			iconDash.SetActive(angle<90);
			iconBlock.SetActive(angle>90);
			dashCd.fillAmount=1-Player.instance.dashCdProgress;

			dashAvailable.SetActive(Player.instance.canDash);
			dashing.SetActive(!Player.instance.canDash);
			blockAvailable.SetActive(!Player.instance.isBlocking);
			blocking.SetActive(Player.instance.isBlocking);

		}
	}

}