using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class SubtitleController:MonoBehaviour {

		public static SubtitleController instance;
		Text text;
		float alpha;

		private void Start() {
			instance=this;
			text=GetComponent<Text>();
		}

		private void Update() {
			alpha-=0.5f*Time.deltaTime;
			text.color=new Color(1,1,1,Mathf.Clamp01(alpha));
		}
		
		public void PushSubtitle(string newText){
			alpha=2;
			text.text=newText;
		}


	}

}