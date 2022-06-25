using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[AddComponentMenu("’Ω∂∑µÿÕº/µ∆π‚…¡À∏")]
public class LightFlicker:MonoBehaviour {

	[SerializeField] AnimationCurve flickerCurve;
	[SerializeField] float flickerPeriod;

	float originalIntensity;
	new Light2D light;
	private void Start() {
		light=GetComponent<Light2D>();
		originalIntensity=light.intensity;
	}

	float timer;
	private void Update() {
		timer+=Time.deltaTime;
		light.intensity=originalIntensity*flickerCurve.Evaluate(timer/flickerPeriod);
		if(timer>=flickerPeriod) timer-=flickerPeriod;
	}

}
