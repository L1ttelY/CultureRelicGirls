using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class ProjectileBase:MonoBehaviour {

		[field: SerializeField] public float gravity { get; protected set; }
		[SerializeField] int penetratePower;
		[SerializeField] float lifeTime;
		[SerializeField] bool doNotRotate;
		[SerializeField] bool doFlip;

		public bool noDamage;

		new Collider2D collider;
		SpriteRenderer spriteRenderer;
		Animator animator;

		protected float time;

		protected DamageModel damage;
		protected Vector2 velocity;
		protected EntityBase target;
		protected bool friendly;

		protected HashSet<EntityBase> hit = new HashSet<EntityBase>();
		protected int timePenetrated;

		private void Start() {
			collider=GetComponent<Collider2D>();
			spriteRenderer=GetComponent<SpriteRenderer>();
			animator=GetComponent<Animator>();
		}

		public virtual void Init(Vector2 position,Vector2 velocity,EntityBase target,bool friendly,DamageModel damage) {

			if(!spriteRenderer) Start();

			transform.position=position;
			this.velocity=velocity;
			this.target=target;
			this.friendly=friendly;
			this.damage=damage;

			nextPosition=position;

			time=0;
			timePenetrated=0;
			hit.Clear();

			animator.SetTrigger("start");
		}

		Vector2 nextPosition;

		protected virtual void Update() {
			transform.position+=(Vector3)velocity*Time.deltaTime;

			time+=Time.deltaTime;
			if(time>lifeTime) ProjectilePool.Store(this);

		}

		protected virtual void FixedUpdate() {

			if(transform.position.y<-1) ProjectilePool.Store(this);


			transform.position=nextPosition;
			velocity+=Vector2.down*Time.deltaTime*gravity;
			nextPosition=(Vector2)transform.position+velocity*Time.deltaTime;

			if(!noDamage){
				int cnt = collider.Cast(velocity,Utility.raycastBuffer,velocity.magnitude*Time.deltaTime);

				for(int i = 0;i<cnt;i++) {

					RaycastHit2D hit = Utility.raycastBuffer[i];
					EntityBase other = hit.collider.GetComponent<EntityBase>();
					if((other is EntityFriendly)==friendly) continue;
					if(other) Hit(other);
				}
			}

			if(doNotRotate) transform.rotation=Quaternion.identity;
			else transform.rotation=((Angle)velocity).quaternion;
			if(doFlip&&velocity.x<0) {
				if(doNotRotate) transform.localScale=new Vector3(-1,1,1);
				else transform.localScale=new Vector3(1,-1,1);
			}
		}

		protected virtual void Hit(EntityBase target) {

			damage.dealerProjectile=this;

			if(penetratePower>timePenetrated) return;
			if(hit.Contains(target)) return;
			hit.Add(target);

			damage.direction=friendly ? Direction.right : Direction.left;
			target.Damage(damage);

			if(penetratePower==timePenetrated) ProjectilePool.Store(this);
			timePenetrated++;

		}

		private void OnDisable() {
			transform.position+=Vector3.down*10f;
		}

	}

}