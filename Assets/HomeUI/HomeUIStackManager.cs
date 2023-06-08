using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Home {

	public class HomeUIStackManager:MonoBehaviour {

		[SerializeField] HomeUIInstance defaultUIInstance;

		public static HomeUIStackManager instance;
		private void Start() {
			if(instance) Destroy(gameObject);
			instance=this;

			if(defaultUIInstance!=null) SetDefault((defaultUIInstance, null));

		}

		Stack<(HomeUIInstance, object)> uiStack = new Stack<(HomeUIInstance, object)>();
		(HomeUIInstance, object) _activeUI;
		(HomeUIInstance, object) _defaultUI;
		public (HomeUIInstance, object) activeUI {
			get { return _activeUI; }
			private set {

				if(value==_activeUI) return;
				if(value.Item1==null&&activeUI==_defaultUI) return;

				if(value.Item1==null) value=_defaultUI;

				value.Item1.OnActivate(value.Item2);
				if(_activeUI.Item1!=null) _activeUI.Item1.gameObject.SetActive(false);
				value.Item1.gameObject.SetActive(true);
				_activeUI=value;

				Debug.Log(_activeUI.Item1);

			}
		}


		//将一个新的UI作为新的栈顶使用
		public void PushUI((HomeUIInstance, object) newUI) {

			uiStack.Push(newUI);
			UpdateActiveUI();

		}

		public void SetDefault((HomeUIInstance, object) defaultUI) {
			_defaultUI=defaultUI;
			UpdateActiveUI();
		}

		void UpdateActiveUI() {
			if(uiStack.Count==0) activeUI=_defaultUI;
			else activeUI=uiStack.Peek();
		}

		public bool CanPopUI() => uiStack.Count>0;
		public bool TryPopUI(){
			if(!CanPopUI()) return false;
			uiStack.Pop();
			UpdateActiveUI();
			return true;
		}

	}

}
