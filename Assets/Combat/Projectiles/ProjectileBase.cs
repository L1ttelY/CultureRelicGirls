using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class ProjectileBase:MonoBehaviour {

		[field:SerializeField] public float gravity { get; protected set; }
		[SerializeField] int penetratePower;
		[SerializeField] float timePerFrame;
		[SerializeField] float lifeTime;
		[SerializeField] Sprite[] sprites;

		new Collider2D collider;
		SpriteRenderer spriteRenderer;

		protected float time;
		protected float timeThisFrame;
		protected int imageIndex;

		protected DamageModel damage;
		protected Vector2 velocity;
		protected EntityBase target;
		protected bool friendly;

		private void Start() {
			collider=GetComponent<Collider2D>();
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		public virtual void Init(Vector2 position,Vector2 velocity,EntityBase target,bool friendly,DamageModel damage) {
			transform.position=position;
			this.velocity=velocity;
			this.target=target;
			this.friendly=friendly;
			this.damage=damage;

			nextPosition=position;

			time=0;
			timeThisFrame=0;
			imageIndex=0;
			if(sprites.Length>0) spriteRenderer.sprite=sprites[0];
		}

		Vector2 nextPosition;

		protected virtual void Update() {
			transform.position+=(Vector3)velocity*Time.deltaTime;

			timeThisFrame+=Time.deltaTime;
			time+=Time.deltaTime;
			if(time>lifeTime) ProjectilePool.Store(this);
			if(timeThisFrame>timePerFrame) {
				if(imageIndex+1<sprites.Length) imageIndex++;
				if(sprites.Length>0) spriteRenderer.sprite=sprites[imageIndex];
				timeThisFrame-=timePerFrame;
			}

		}

		protected virtual void FixedUpdate() {

			transform.position=nextPosition;
			velocity+=Vector2.down*Time.deltaTime*gravity;
			nextPosition=(Vector2)transform.position+velocity*Time.deltaTime;

			int cnt = collider.Cast(velocity,Utility.raycastBuffer,velocity.magnitude*Time.deltaTime);

			for(int i = 0;i<cnt;i++) {

				RaycastHit2D hit = Utility.raycastBuffer[i];
				EntityBase other = hit.collider.GetComponent<EntityBase>();
				if((other is EntityFriendly)==friendly) continue;
				if(other) {
					damage.direction=friendly ? Direction.right : Direction.left;
					other.Damage(damage);
				}
			}

		}

	}

}