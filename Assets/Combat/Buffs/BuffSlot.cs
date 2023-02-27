using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class BuffSlot {

		EntityBase owner;

		Dictionary<System.Type,BuffBase> buffs = new Dictionary<System.Type,BuffBase>();

		public BuffBase this[System.Type index] {
			get {
				if(buffs.ContainsKey(index)) return buffs[index];
				else {
					BuffBase newBuff = index.GetConstructor(new System.Type[0]).Invoke(new object[0]) as BuffBase;
					newBuff.Init(owner,this);
					buffs.Add(index,newBuff);
					return newBuff;
				}
			}
		}

		public bool ContainsBuff(System.Type buffType) => buffs.ContainsKey(buffType);

		/// <summary>
		/// 只应在Buff.RemoveSelf一处中调用
		/// </summary>
		public void RemoveBuff(System.Type target) {
			buffs.Remove(target);
		}

		public void Init(EntityBase owner) {
			this.owner=owner;
		}

		/// <summary>
		/// 只应在EntityBase.Update中调用
		/// </summary>
		public void Update() {
			var i = buffs.GetEnumerator();

			while(buffs.Count!=0) {
				var current = i.Current.Value;
				bool doBreak = !i.MoveNext();
				if(current!=null)current.Update();
				if(doBreak) break;
			}

		}
	}

}