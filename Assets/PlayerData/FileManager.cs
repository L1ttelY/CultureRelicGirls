using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public static class FileManager {

	static string SAPath {
		get {
			if(Application.platform==RuntimePlatform.Android) return "jar:file://"+Application.dataPath+"!/assets";
			else return Application.streamingAssetsPath;
		}
	}

	public static string ReadSA(string fileName) {

		if(Application.platform==RuntimePlatform.Android) {
			UnityWebRequest req = UnityWebRequest.Get(SAPath+"/"+fileName);
			req.SendWebRequest();
			while(!req.isDone) ;
			return req.downloadHandler.text;
		} else {
			return File.ReadAllText(SAPath+"/"+fileName);
		}

	}

	public static string activeDataPath {
		get {
#if UNITY_ANDROID
			return Application.persistentDataPath;
#endif
			return Application.dataPath;
		}
	}

}