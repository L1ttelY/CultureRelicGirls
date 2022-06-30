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
	public static class PlayerDataController {

		const string fileName = "save.xml";
		public static bool loaded { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		static void Init() {


			//直接读取存档
			string serialized = "";
			if(File.Exists(SavePath)) serialized=File.ReadAllText(SavePath);
			else serialized=FileManager.ReadSA(fileName);
			//else serialized=Resources.Load<Museum.StoryData>("initial save").text;

			SerializedToMemory(serialized);

			SaveGame();

		}


		static public void SaveGame() {
			string save = MemoryToSerialized();
			if(!File.Exists(SavePath)) File.Create(SavePath).Dispose();
			File.WriteAllText(SavePath,MemoryToSerialized());
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

		static string SavePath => FileManager.activeDataPath+"/"+fileName;

	}

}