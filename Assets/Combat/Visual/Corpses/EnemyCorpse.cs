using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyCorpse:MonoBehaviour {

		static GameObject prefab;
		public static EnemyCorpse Create(Sprite []corpseSprites,Vector2 position,float timePerFrame,int direction) {
			if(!prefab) prefab=Resources.Load<GameObject>("EnemyCorpsePrefab");
			EnemyCorpse result = Instantiate(prefab,position,Quaternion.identity).GetComponent<EnemyCorpse>();
			result.Init(corpseSprites,timePerFrame,direction);
			return result;
		}

		SpriteRenderer spriteRenderer;
		Sprite[] sprites;
		float timePerFrame;

		private void Init(Sprite[] corpseSprites,float timePerFrame,int direction) {
			spriteRenderer=GetComponent<SpriteRenderer>();
			sprites=corpseSprites;
			spriteRenderer.sprite=sprites[0];
			this.direction=direction;
			this.timePerFrame=timePerFrame;
			spriteRenderer.flipX=direction==Direction.left;
		}

		int direction;
		float time;
		[SerializeField] float speed = 5;
		[SerializeField] float friction = 3;
		[SerializeField] float lifetime = 35;

		int imageIndex=0;

		const float fallSpeed = 5;

		void Update() {

			float floorY = transform.parent.position.y;

			Vector3 deltaPosition = -(Vector3)Direction.GetVector(direction)*speed*Time.deltaTime;
			//if(transform.position.y>floorY+Time.deltaTime*fallSpeed) deltaPosition+=Vector3.down*Time.deltaTime*fallSpeed;
			//else deltaPosition.y=floorY-transform.position.y;
			transform.position+=deltaPosition;
			speed=Mathf.Max(0,speed-friction*Time.deltaTime);

			time+=Time.deltaTime;

			if(imageIndex<sprites.Length-1&&(imageIndex+1)*timePerFrame<time) imageIndex++;
			spriteRenderer.sprite=sprites[imageIndex];

			if(time>lifetime-1) spriteRenderer.color=new Color(1,1,1,lifetime-time);
			if(time>lifetime) Destroy(gameObject);

		}

	}

}