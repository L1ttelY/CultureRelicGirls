using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

namespace PlayerData {
	/// <summary>
	/// ����浵�Ķ�ȡ�ͱ���Ĺ���
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