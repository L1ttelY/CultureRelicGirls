using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {
	public class BuildingWithCharacterInteractionBase:BuildingControllerBase {

		public class SlotToken {
			public readonly Vector2 position;
			public readonly int index;
			public BuildingWithCharacterInteractionBase owner;
			public SlotToken(Vector2 position,int index,BuildingWithCharacterInteractionBase owner) {
				this.position=position;
				this.index=index;
				this.owner=owner;
			}
		}

		[SerializeField] Vector2[] slots;
		bool[] slotOccupied;

		protected override void Start() {
			base.Start();
			slotOccupied=new bool[slots.Length];
		}

		public SlotToken GetSlot() {

			for(int i = 0;i<slots.Length;i++) {
				if(!slotOccupied[i]) {
					slotOccupied[i]=true;
					return new SlotToken((Vector2)transform.position+slots[i],i,this);
				}
			}

			return null;

		}


		public SlotToken GetStaticSlot() {
			return new SlotToken(transform.position,-1,null);
		}

		public void FreeSlot(SlotToken token) {
			if(token==null) return;
			if(token.owner!=this) return;
			slotOccupied[token.index]=false;
		}
		public virtual bool HasSlotLeft() {
			for(int i = 0;i<slots.Length;i++) {
				if(!slotOccupied[i]) return true;
			}
			return false;
		}

		static Color[] colors ={
			Color.red,
			new Color(.9f,.4f,.1f),
			Color.yellow,
			Color.green,
			Color.blue,
			new Color(.2f,0,.5f),
			new Color(.4f,0,.5f)
		};

		private void OnDrawGizmos() {

			for(int i = 0;i<slots.Length;i++) {
				Gizmos.color=colors[i];
				Gizmos.DrawCube(transform.position+(Vector3)slots[i],Vector3.one*0.2f);
			}
		}

	}
}