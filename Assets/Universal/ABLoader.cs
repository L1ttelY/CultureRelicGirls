using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ABLoader {

	static bool loaded;

	[RuntimeInitializeOnLoadMethod]
	static void LoadBundles() {

		if(loaded) return;
		loaded=true;

		ItemData.ClearInstances();
		AssetBundle ab;
		ab=FileManager.LoadSAAB("stationdata.ab"); ab.LoadAllAssets();

		ab=FileManager.LoadSAAB("itemdata.ab"); ab.LoadAllAssets();
		string[] a = ab.GetAllAssetNames();
		foreach(var i in a) Debug.LogWarning(a);
		Debug.LogWarning("FINNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
	}

}
