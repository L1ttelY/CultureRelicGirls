using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

namespace PlayerData {
	/// <summary>
	/// 负责存档的读取和保存的管理
	/// </summary>
	public static class PlayerDataController {

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			WX.InitSDK(SDKInited);

		}

		static void SDKInited(int _) {

			AccessParam accessParam = new AccessParam();
			accessParam.path="/save";

			fileSystem=WX.GetFileSystemManager();
			fileSystem.Access(accessParam);

		}


		static

	}

}