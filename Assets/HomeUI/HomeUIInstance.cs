using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Home {

	public class HomeUIInstance:MonoBehaviour {

		public static Dictionary<string,HomeUIInstance> instances = new Dictionary<string,HomeUIInstance>();

		virtual protected void Start() {
			//if(instances.ContainsKey(GetType())&&instances[GetType()]) Destroy(gameObject);
			instances[gameObject.name]=this;
			if(HomeUIStackManager.instance&&this==HomeUIStackManager.instance.activeUI.Item1==this) {
			} else gameObject.SetActive(false);
		}

		public virtual void OnActivate(object workingData) {

		}

	}

}
