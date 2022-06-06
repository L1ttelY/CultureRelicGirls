using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

namespace PlayerData {

	public static class PlayerData {

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {
			WX.InitSDK(SDKInited);

		}

		static void SDKInited(int _) {

			AccessParam accessParam =new AccessParam();
			accessParam.path="/save";

			fileSystem=WX.GetFileSystemManager();
			fileSystem.Access(accessParam);
			
		}


	}

}