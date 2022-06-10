using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

namespace PlayerData {
	/// <summary>
	/// 负责存档的读取和保存的管理
	/// </summary>
	public static class PlayerDataController {

		static string filePath = "/save";

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {

			if(Utility.debug) {
				//直接读取存档

			} else {
				//初始化微信SDK
				WX.InitSDK(SDKInited);
			}

		}


		static void SDKInited(int _) {

			fileSystem=WX.GetFileSystemManager();

			fileSystem.AccessSync(filePath);


		}


		static void LoadSave() {

		}

		public static void SaveSave(){
			
		}

	}

}