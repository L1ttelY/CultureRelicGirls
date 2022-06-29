using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioController:MonoBehaviour {

	AudioSource audioSource;
	AudioClip activeClip;

	private void Update() {
		if(!audioSource.isPlaying) {
			pool.Push(this);
			gameObject.SetActive(false);
		}
	}

	private void OnEnable() {
		audioSource=GetComponent<AudioSource>();
		if(activeClip!=null) audioSource.PlayOneShot(activeClip);
	}

}

//static
public partial class AudioController:MonoBehaviour {

	static GameObject prefab;
	static Stack<AudioController> pool = new Stack<AudioController>();

	public static void PlayAudio(AudioClip clip,Vector2 position) {

		if(prefab==null) prefab=Resources.Load<GameObject>("AudioPlayer");

		AudioController newController;
		if(pool.Count>0) {
			newController=pool.Pop();
		} else {
			newController=Instantiate(prefab,Vector3.zero,Quaternion.identity).GetComponent<AudioController>();
			DontDestroyOnLoad(newController.gameObject);
			newController.gameObject.SetActive(false);
		}

		newController.transform.position=position;
		newController.activeClip=clip;
		newController.gameObject.SetActive(true);

	}

}