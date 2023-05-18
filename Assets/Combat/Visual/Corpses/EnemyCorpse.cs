using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class EnemyCorpse:MonoBehaviour {

		static GameObject prefab;
		public static EnemyCorpse Create(Sprite[] corpseSprites,Vector2 position,float timePerFrame,int direction,float groundY) {
			if(!prefab) prefab=Resources.Load<GameObject>("EnemyCorpsePrefab");
			EnemyCorpse result = Instantiate(prefab,position,Quaternion.identity).GetComponent<EnemyCorpse>();
			result.Init(corpseSprites,timePerFrame,direction,groundY);
			return result;
		}

		SpriteRenderer spriteRenderer;
		Sprite[] sprites;
		float timePerFrame;

		private void Init(Sprite[] corpseSprites,float timePerFrame,int direction,float groundY) {
			spriteRenderer=GetComponent<SpriteRenderer>();
			sprites=corpseSprites;
			spriteRenderer.sprite=sprites[0];
			this.direction=direction;
			this.timePerFrame=timePerFrame;
			spriteRenderer.flipX=direction==Direction.left;
			this.groundY=groundY;
		}

		float groundY;
		float velocityY;
		int direction;
		float time;
		[SerializeField] float speed = 5;
		[SerializeField] float friction = 3;
		[SerializeField] float lifetime = 35;

		int imageIndex = 0;

		const float gravity = 2;
		const float velocityYMax = 5;

		void Update() {

			float groundY = transform.parent.position.y+this.groundY;
			Vector3 deltaPosition = -(Vector3)Direction.GetVector(direction)*speed*Time.deltaTime;
			velocityY+=gravity*Time.deltaTime;
			if(velocityY>velocityYMax) velocityY=velocityYMax;

			deltaPosition.y-=velocityY;

			transform.position+=deltaPosition;
			if(transform.position.y<groundY){
				Vector3 pos = transform.position;
				pos.y=groundY;
				transform.position=pos;
			}
			speed=Mathf.Max(0,speed-friction*Time.deltaTime);

			time+=Time.deltaTime;

			if(imageIndex<sprites.Length-1&&(imageIndex+1)*timePerFrame<time) imageIndex++;
			spriteRenderer.sprite=sprites[imageIndex];

			if(time>lifetime-1) spriteRenderer.color=new Color(1,1,1,lifetime-time);
			if(time>lifetime) Destroy(gameObject);

		}

	}

}