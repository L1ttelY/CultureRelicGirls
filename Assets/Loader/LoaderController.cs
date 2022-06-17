using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderController:MonoBehaviour {

	[SerializeField] string bundleName;

	void Start() {

		AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/"+bundleName+".ab");
		AssetBundle sceneBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/"+bundleName+".sab");
		string[] scenePaths=sceneBundle.GetAllScenePaths();
		SceneManager.LoadScene(scenePaths[0]);

	}

}
