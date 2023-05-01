using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class ActionButtonController:MonoBehaviour {

		[SerializeField] GameObject iconDash;
		[SerializeField] GameObject iconBlock;
		[SerializeField] Image dashCd;

		void Start() {

		}

		float angle = 180;

		void Update() {
			
			bool front = Mathf.Abs(Player.instance.targetVelocity)>0.5f;
			float deltaAngle = 1000*Time.deltaTime;
			if(front) {
				if(angle>deltaAngle) angle-=deltaAngle;
				else angle=0;
			}else{
				if(angle<180-deltaAngle) angle+=deltaAngle;
				else angle=180;
			}
			
			Quaternion rotation = Quaternion.Euler(angle,0,0);

			transform.rotation=rotation;
			
			iconDash.SetActive(angle<90);
			iconBlock.SetActive(angle>90);
			dashCd.fillAmount=1-Player.instance.dashCdProgress;
			
		}
	}

}