using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager:MonoBehaviour {

	public static event Void staticUpdate;
	static EventManager instance;

	void Start() {
		if(instance) {
			Destroy(gameObject);
			return;
		}
		instance=this;
		DontDestroyOnLoad(gameObject);
	}

	void Update() {
		staticUpdate?.Invoke();
	}
}
