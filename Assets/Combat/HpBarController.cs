using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class HpBarController:MonoBehaviour {

		EntityBase entity;
		Image image;

		void Start() {

			entity=GetComponentInParent<EntityBase>();
			image=GetComponent<Image>();

		}

		void Update() {

			Vector3 parentScale = transform.parent.lossyScale;
			transform.localScale=new Vector3(1/parentScale.x,1/parentScale.y,1);


			image.fillAmount=((float)entity.hp)/((float)entity.maxHp);

		}

	}

}