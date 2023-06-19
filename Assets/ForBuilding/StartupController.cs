using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StartupController:MonoBehaviour {

	int stage;
	[SerializeField] VideoPlayer video;

	[SerializeField] float clickDelay;
	[SerializeField] float totalTime;
	float timeAfterClick;
	float timeAfterPlay;

	private void Update() {
		timeAfterClick+=Time.deltaTime;
		if(Input.GetMouseButtonDown(0)&&timeAfterClick>clickDelay) {
			stage++;
			timeAfterClick=0;
			if(stage==1) {
				video.gameObject.SetActive(true);
				video.Play();
			}
			if(stage==2) SceneManager.LoadScene("VehicleScene");

		}

		if(stage>=1&&timeAfterPlay>totalTime){
			SceneManager.LoadScene("VehicleScene");
		}

	}

}
