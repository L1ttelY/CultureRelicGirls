using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioController:MonoBehaviour {

	AudioSource audioSource;
	private void Start() {
		audioSource=GetComponent<AudioSource>();
	}

	private void Update() {
		if(!audioSource.isPlaying) {
			pool.Push(this);
			gameObject.SetActive(false);
		}
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
			newController.gameObject.SetActive(true);
		} else {
			newController=Instantiate(prefab,Vector3.zero,Quaternion.identity).GetComponent<AudioController>();
			DontDestroyOnLoad(newController.gameObject);
		}

		newController.transform.position=position;
		newController.gameObject.SetActive(true);
		newController.audioSource.PlayOneShot(clip);

	}

}