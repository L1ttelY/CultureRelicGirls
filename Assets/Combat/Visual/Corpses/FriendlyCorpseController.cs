using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class FriendlyCorpseController:MonoBehaviour {

		public static void Create(Transform transform,Sprite sprite) {
			if(!prefab) prefab=Resources.Load<GameObject>("FriendlyCorpsePrefab");
			GameObject go = Instantiate(prefab,transform.position,transform.rotation);
			go.transform.localScale=transform.localScale;
			go.GetComponent<SpriteRenderer>().sprite=sprite;
		}
		static GameObject prefab;

		Vector2 velocity;
		float rotationSpeed;

		private void Start() {
			velocity=new Vector2(Random.Range(-17,-13),Random.Range(13,7));
			rotationSpeed=Random.Range(70,150);
		}

		void Update() {
			
			velocity-=0.3f*velocity*Time.deltaTime;
			velocity+=Vector2.down*Time.deltaTime;

			rotationSpeed=Mathf.Max(0,rotationSpeed-rotationSpeed*Time.deltaTime);

			Quaternion rot = transform.rotation;
			rot=Quaternion.Euler(rot.eulerAngles+Vector3.forward*rotationSpeed*Time.deltaTime);
			transform.rotation=rot;

			transform.position+=(Vector3)velocity*Time.deltaTime;

			if((transform.position-CameraController.instance.transform.position).sqrMagnitude>200) Destroy(gameObject);

		}



	}

}