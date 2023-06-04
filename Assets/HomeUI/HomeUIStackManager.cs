using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Home {

	public class HomeUIStackManager:MonoBehaviour {

		public static HomeUIStackManager instance;
		private void Start() {
			if(instance) Destroy(gameObject);
			instance=this;
		}

		Stack<(HomeUIInstance, object)> UIStack = new Stack<(HomeUIInstance, object)>();
		(HomeUIInstance, object) _activeUI;
		(HomeUIInstance, object) _defaultUI;
		public (HomeUIInstance, object) activeUI {
			get { return _activeUI; }
			private set {
				if(value!=_activeUI) {

					if(value.Item1==null) value=_defaultUI;

					value.Item1.OnActivate(value.Item2);
					_activeUI.Item1.gameObject.SetActive(false);
					value.Item1.gameObject.SetActive(true);
					_activeUI=value;

				}
			}
		}


		//将一个新的UI作为新的栈顶使用
		public void PushUI((HomeUIInstance, object) newUI) {

			UIStack.Push(newUI);
			activeUI=newUI;

		}

		public void SetDefault((HomeUIInstance, object) defaultUI) {
			_defaultUI=defaultUI;
		}


	}

}
