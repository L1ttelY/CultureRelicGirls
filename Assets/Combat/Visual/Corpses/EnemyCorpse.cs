using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyCorpse:MonoBehaviour {

		static GameObject prefab;
		public static EnemyCorpse Create(Sprite corpseSprite,Vector2 position,int direction) {
			if(!prefab) prefab=Resources.Load<GameObject>("EnemyCorpsePrefab");
			EnemyCorpse result = Instantiate(prefab,position,Quaternion.identity).GetComponent<EnemyCorpse>();
			result.Init(corpseSprite,direction);
			return result;
		}

		SpriteRenderer spriteRenderer;


		private void Init(Sprite sprite,int direction) {
			spriteRenderer=GetComponent<SpriteRenderer>();
			spriteRenderer.sprite=sprite;
			this.direction=direction;
			spriteRenderer.flipX=direction==Direction.left;
		}

		int direction;
		float time;
		[SerializeField] float speed = 5;
		[SerializeField] float friction = 3;
		[SerializeField] float lifetime = 35;

		void Update() {

			transform.position-=(Vector3)Direction.GetVector(direction)*speed*Time.deltaTime;
			speed=Mathf.Max(0,speed-friction*Time.deltaTime);

			time+=Time.deltaTime;
			if(time>lifetime-1) spriteRenderer.color=new Color(1,1,1,lifetime-time);
			if(time>lifetime) Destroy(gameObject);

		}

	}

}