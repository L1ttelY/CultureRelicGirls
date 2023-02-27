using UnityEditor;
using System.IO;

public class BundlePacker {
  [MenuItem("Assets/Build AssetBundles")]
  static void BuildAllAssetBundles() {
    string assetBundleDirectory = "Assets/StreamingAssets";
    if(!Directory.Exists(assetBundleDirectory)) {
      Directory.CreateDirectory(assetBundleDirectory);
    }
    BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                    BuildAssetBundleOptions.None,
                                    BuildTarget.StandaloneWindows);
  }
}