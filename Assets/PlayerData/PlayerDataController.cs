using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

namespace PlayerData {
	/// <summary>
	/// ����浵�Ķ�ȡ�ͱ���Ĺ���
	/// </summary>
	public static class PlayerDataController {

		static string filePath = "/save";

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {

			if(Utility.debug) {
				//ֱ�Ӷ�ȡ�浵

			} else {
				//��ʼ��΢��SDK
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