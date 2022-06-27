using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Museum {

	/// <summary>
	/// 在楼层等级满足一定条件时显示
	/// </summary>
	[AddComponentMenu("博物馆/在特定楼层等级显示")]
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
			throw new System.NotImplementedException();

			bool display = false;
			int currentLevel = 0;

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