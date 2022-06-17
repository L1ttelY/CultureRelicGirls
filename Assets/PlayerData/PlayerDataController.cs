using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System.Xml;
using System.IO;
using System.Text;

namespace PlayerData {
	/// <summary>
	/// ����浵�Ķ�ȡ�ͱ���Ĺ���
	/// </summary>
	public static class PlayerDataController {

		static string fileName = "save.xml";

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {

			if(Utility.debug) {
				//ֱ�Ӷ�ȡ�浵
				string serialized = "";
				if(File.Exists(EditorPath)) serialized=File.ReadAllText(EditorPath);
				SerializedToMemory(serialized);
				SaveGame();
			} else {
				//��ʼ��΢��SDK
				WX.InitSDK(SDKInited);

			}

		}


		static void SDKInited(int _) {
			fileSystem=WX.GetFileSystemManager();
			string serialized = WX.StorageGetStringSync(fileName,"");
			SerializedToMemory(serialized);
			SaveGame();
		}

		public static void SaveGame() {
			string save = MemoryToSerialized();
			if(Utility.debug) {
				if(!File.Exists(EditorPath)) File.Create(EditorPath);
				File.WriteAllText(EditorPath,MemoryToSerialized());
			} else {
				WX.StorageSetStringSync(fileName,MemoryToSerialized());
			}
		}


		static void SerializedToMemory(string data) {
			XmlDocument xml = new XmlDocument();
			if(data.Length!=0) xml.LoadXml(data);
			PlayerData.LoadDocument(xml);
		}

		static string MemoryToSerialized() {
			XmlDocument xml = new XmlDocument();
			PlayerData.SaveDocument(xml);
			return xml.InnerXml;
		}
			
		static string EditorPath { get { return Application.dataPath+"\\"+fileName; } }

	}

}