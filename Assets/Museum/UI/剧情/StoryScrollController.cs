using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	public class StoryScrollController:MonoBehaviour {

		[SerializeField] Transform pos1;
		[SerializeField] Transform pos2;

		StoryMode owner;
		private void Start() {
			owner=GetComponentInParent<StoryMode>();
		}

		private void Update() {
			transform.position=Vector3.Lerp(pos1.position,pos2.position,owner.scrollRatio);
		}

	}

}