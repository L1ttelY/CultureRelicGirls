using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Home{

  public class HomeUIInstance:MonoBehaviour {

		public static Dictionary<System.Type,HomeUIInstance> instances=new Dictionary<System.Type, HomeUIInstance>();

		virtual protected void Start() {
			instances[GetType()]=this;
		}

		public virtual void OnActivate(object workingData){
      
		}

  }

}
