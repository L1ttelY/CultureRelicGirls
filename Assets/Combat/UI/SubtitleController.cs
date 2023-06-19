using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat {

	public class SubtitleController:MonoBehaviour {

		[SerializeField] int id;
		[SerializeField] float curveLength;
		[SerializeField] AnimationCurve alphaCurve;

		public static Dictionary<int,SubtitleController> instances=new Dictionary<int, SubtitleController>();
		Text text;
		float timer=99999;

		private void Start() {
			instances[id]=this;
			text=GetComponent<Text>();
		}

		private void Update() {
			timer+=Time.deltaTime;

			float alpha = 0;
			if(timer<curveLength) alpha=alphaCurve.Evaluate(timer/curveLength);
			text.color=new Color(1,1,1,Mathf.Clamp01(alpha));
		}
		
		public void PushSubtitle(string newText){
			timer=0;
			text.text=newText;
		}


	}

}