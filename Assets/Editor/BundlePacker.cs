using UnityEditor;
using System.IO;

public class BundlePacker {
  [MenuItem("Assets/Build AssetBundles Windows")]
  static void BuildAllAssetBundlesWindows() {
    string assetBundleDirectory = "Assets/StreamingAssets";
    if(!Directory.Exists(assetBundleDirectory)) {
      Directory.CreateDirectory(assetBundleDirectory);
    }
    BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                    BuildAssetBundleOptions.None,
                                    BuildTarget.StandaloneWindows);
  }

  [MenuItem("Assets/Build AssetBundles Android")]
  static void BuildAllAssetBundlesAndroid() {
    string assetBundleDirectory = "Assets/StreamingAssets";
    if(!Directory.Exists(assetBundleDirectory)) {
      Directory.CreateDirectory(assetBundleDirectory);
    }
    BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                    BuildAssetBundleOptions.None,
                                    BuildTarget.Android);
  }
}