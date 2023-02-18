using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class BuffBase {

		protected EntityBase owner;
		protected BuffSlot ownerSlot;

		protected float _stacks;
		public virtual float stacks {
			get => _stacks;
			set {
				_stacks=value;
			}
		}

		/// <summary>
		/// НігІдк
		/// </summary>
		public virtual void Init(EntityBase owner,BuffSlot ownerSlot) {
			this.owner=owner;
			this.ownerSlot=ownerSlot;
		}

		protected virtual void RemoveSelf() {
			ownerSlot.RemoveBuff(GetType());
		}

		public virtual void Update(){

		}

	}

}