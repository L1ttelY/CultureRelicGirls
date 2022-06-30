using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound:MonoBehaviour {

	[SerializeField] AudioClip audioClip;

	private void Start() {
		Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();

		foreach(var i in buttons) {
			i.onClick.AddListener(PlaySound);
		}
		instance=this;
	}

	public static ButtonSound instance { get; private set; }

	public void PlaySound() {
		AudioController.PlayAudio(audioClip,Camera.main.transform.position);
	}

}
