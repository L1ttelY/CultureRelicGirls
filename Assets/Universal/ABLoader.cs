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
		AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/itemdata.ab").LoadAllAssets();

	}

}
