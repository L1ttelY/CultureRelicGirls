using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandler(object sender);
public delegate void Void();

namespace Combat {

	public struct DamageModel{
		public int amount;
		public float knockback;
		public EntityBase dealer;
		public int direction;
	}

	public class EntityBase:MonoBehaviour {

		[SerializeField] bool debug;
		public static LinkedList<EntityBase> entities=new LinkedList<EntityBase>();
		LinkedListNode<EntityBase> positionInList;

		//属性
		[field: SerializeField] public float acceleration { get; protected set; }
		[field: SerializeField] public float maxSpeed { get; protected set; }
		[field: SerializeField] public int attackBasePower { get; protected set; }
		[field: SerializeField] public float attackCd { get; protected set; }
		[field: SerializeField] public float knockbackPower { get; protected set; }
		[field: SerializeField] public int maxHp { get; protected set; }

		//当前状态
		public int hp { get; protected set; }
		public float powerBuff;
		public float cdSpeed;
		public float speedBuff;

		int direction = Direction.right;

		public static event EventHandler UpdateStats;
		void OnUpdateStats() {
			powerBuff=1;
			cdSpeed=1;
			speedBuff=1;
			UpdateStats?.Invoke(this);
		}

		public virtual void Damage(DamageModel e) {
			hp-=e.amount;
			StartKnockback(e.knockback);
			if(hp<=0) OnDeath();
		}

		public static event EventHandler Death;
		protected virtual void OnDeath() {
			Death?.Invoke(this);
		}

		protected virtual void Start() {
			previousPosition=transform.position;
			StartMove();
			positionInList=entities.AddLast(this);
		}

		protected virtual void OnDestroy() {
			entities.Remove(positionInList);
		}

		private void Update() {
			if(debug&&Input.GetKeyDown(KeyCode.W)) StartKnockback(1);
			UpdateMove();
		}

		protected virtual void FixedUpdate() {
			OnUpdateStats();
			currensState();
		}

		//移动相关
		protected Vector2 velocity;
		protected Vector2 previousPosition;
		virtual protected void UpdateMove() {
			Vector2 position = transform.position;
			position+=velocity*Time.deltaTime;
			if(position.y<0) position.y=0;
			transform.position=position;
		}

		protected Void currensState;

		protected virtual void StartMove() {
			currensState=StateMove;
			velocity=Vector2.zero;
		}
		protected virtual void StateMove() {

		}

		protected virtual void StartKnockback(float knockback) {
			timeSinceKnockback=0;
			currentKnockback=knockback;
			knockbackDirection=Direction.Reverse(direction);
			currensState=StateKnockback;
		}

		const float knockbackTime = 0.1f;
		const float knockbackHeight = 0.5f;

		float currentKnockback;
		float timeSinceKnockback;
		int knockbackDirection;
		protected virtual void StateKnockback() {
			timeSinceKnockback+=Time.deltaTime;
			Vector2 position = previousPosition;

			float curveX = (timeSinceKnockback-0.5f*knockbackTime)/knockbackTime;
			position.y=knockbackHeight*(-curveX*curveX+0.25f);
			position+=Direction.GetVector(knockbackDirection)*Time.deltaTime*currentKnockback/knockbackTime;


			curveX=(timeSinceKnockback+Time.deltaTime-0.5f*knockbackTime)/knockbackTime;
			float nextY = knockbackHeight*(-curveX*curveX+0.25f);

			velocity=Direction.GetVector(knockbackDirection)*currentKnockback/knockbackTime;
			velocity.y=(nextY-position.y)/Time.deltaTime;

			transform.position=position;
			previousPosition=position;
			direction=Direction.Reverse(knockbackDirection);
			if(timeSinceKnockback>=knockbackTime) StartMove();
		}

	}

}