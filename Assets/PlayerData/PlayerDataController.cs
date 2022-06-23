using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

namespace PlayerData {
	/// <summary>
	/// 负责存档的读取和保存的管理
	/// </summary>
	public static class PlayerDataController{

		const string fileName = "save.xml";
		public static bool loaded { get; private set; }

		public static WXFileSystemManager fileSystem;
		[RuntimeInitializeOnLoadMethod]
		static void Init() {

			//直接读取存档
			string serialized = "";
			if(File.Exists(EditorPath)) serialized=File.ReadAllText(EditorPath);
			SerializedToMemory(serialized);
			SaveGame();

		}


		static void SDKInited(int _) {
			fileSystem=WX.GetFileSystemManager();
			string serialized = WX.StorageGetStringSync(fileName,"");
			SerializedToMemory(serialized);
			SaveGame();
		}

		static public void SaveGame() {
			string save = MemoryToSerialized();
			if(!File.Exists(EditorPath)) File.Create(EditorPath);
			File.WriteAllText(EditorPath,MemoryToSerialized());
		}

		static void SerializedToMemory(string data) {
			XmlDocument xml = new XmlDocument();
			if(data.Length!=0) xml.LoadXml(data);
			PlayerDataRoot.LoadDocument(xml);
			loaded=true;
		}

		static string MemoryToSerialized() {
			XmlDocument xml = new XmlDocument();
			PlayerDataRoot.SaveDocument(xml);
			return xml.InnerXml;
		}

		static string EditorPath { get { return Application.dataPath+"/"+fileName; } }

	}

}