using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class AvatarCoordinator:MonoBehaviour {

		[SerializeField] AvatarController[] avatars;
		[SerializeField] GameObject[] frames;

		bool loaded;

		private void Update() {

			if(loaded) return;
			loaded=true;
			
			int total = 0;
			for(int i = 0;i<3;i++) {
				if(EntityFriendly.friendlyList[i]) {
					Debug.Log($"{total} -> {i} ; {EntityFriendly.friendlyList[i].gameObject.name}");
					avatars[total].targetIndex=i;
					total++;
				}
			}
			for(int i = 0;i<total;i++) avatars[i].gameObject.SetActive(true);
			
			//if(total>0) frames[total-1].SetActive(true);
			
		}

	}

}