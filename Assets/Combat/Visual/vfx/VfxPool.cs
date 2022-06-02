using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat{

	public static class VfxPool{


		public static Dictionary<string,Stack<VfxController>> pools = new Dictionary<string,Stack<VfxController>>();

		public static VfxController Create(GameObject prefab,Vector2 position,int direction) {
			if(!pools.ContainsKey(prefab.name)) {
				pools[prefab.name]=new Stack<VfxController>();
			}

			VfxController result;

			if(pools[prefab.name].Count==0) {
				GameObject newGo = Object.Instantiate(prefab);
				result=newGo.GetComponent<VfxController>();
				newGo.name=prefab.name;
				Object.DontDestroyOnLoad(newGo);
			} else {
				result=pools[prefab.name].Pop();
			}

			result.gameObject.SetActive(true);
			result.Init(position,direction);
			return result;

		}

		public static void Store(VfxController vfx) {
			string name = vfx.gameObject.name;
			if(!pools.ContainsKey(name)) {
				pools[name]=new Stack<VfxController>();
			}

			pools[name].Push(vfx);
			vfx.gameObject.SetActive(false);

		}


	}

}