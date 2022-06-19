using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class FloorDisplayAtCertainLevel:MonoBehaviour {

		public enum Compar {
			Equal,
			LessThan,
			MoreThan
		}

		[SerializeField] Compar compar;
		[SerializeField] int level;

		SpriteRenderer spriteRenderer;

		void Start() {
			spriteRenderer=GetComponent<SpriteRenderer>();
		}

		void Update() {
			bool display = false;
			int currentLevel = PlayerData.PlayerDataRoot.instance.floorLevel.value;

			switch(compar) {
			case Compar.Equal:
				display=(level==currentLevel);
				break;
			case Compar.LessThan:
				display=(level<currentLevel);
				break;
			case Compar.MoreThan:
				display=(level>currentLevel);
				break;
			}

			spriteRenderer.color=display ? Color.white : Color.clear;

		}
	}
}