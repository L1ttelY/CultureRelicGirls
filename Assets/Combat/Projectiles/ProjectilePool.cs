using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public static class ProjectilePool {

		public static Dictionary<string,Stack<ProjectileBase>> pools=new Dictionary<string, Stack<ProjectileBase>>();

		//¥¥‘Ï∑…µØ
		public static ProjectileBase Create(GameObject prefab,Vector2 position,Vector2 velocity,EntityBase target,bool friendly,DamageModel damage,float manaGain=0) {
			if(!pools.ContainsKey(prefab.name)) {
				pools[prefab.name]=new Stack<ProjectileBase>();
			}

			ProjectileBase result;

			if(pools[prefab.name].Count==0) {
				GameObject newGo = Object.Instantiate(prefab);
				result=newGo.GetComponent<ProjectileBase>();
				newGo.name=prefab.name;
				Object.DontDestroyOnLoad(newGo);
			} else{
				result=pools[prefab.name].Pop();
			}

			result.gameObject.SetActive(true);
			result.Init(position,velocity,target,friendly,damage,manaGain);
			return result;

		}

		public static void Store(ProjectileBase projectile){
			string name = projectile.gameObject.name;
			if(!pools.ContainsKey(name)) {
				pools[name]=new Stack<ProjectileBase>();
			}

			pools[name].Push(projectile);
			projectile.gameObject.SetActive(false);

		}

	}

}