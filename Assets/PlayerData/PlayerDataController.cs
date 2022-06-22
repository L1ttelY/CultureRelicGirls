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
	/// ����浵�Ķ�ȡ�ͱ���Ĺ���
	/// </summary>
	public class PlayerDataController:MonoBehaviour {

		const string fileName = "save.xml";

		public static WXFileSystemManager fileSystem;
		void Awake() {

			//ֱ�Ӷ�ȡ�浵
			string serialized = "";
			if(File.Exists(EditorPath)) serialized=File.ReadAllText(EditorPath);
			SerializedToMemory(serialized);
			SaveGame();

		}


		void SDKInited(int _) {
			fileSystem=WX.GetFileSystemManager();
			string serialized = WX.StorageGetStringSync(fileName,"");
			SerializedToMemory(serialized);
			SaveGame();
		}

		public void SaveGame() {
			string save = MemoryToSerialized();
			if(!File.Exists(EditorPath)) File.Create(EditorPath);
			File.WriteAllText(EditorPath,MemoryToSerialized());
		}

		void SerializedToMemory(string data) {
			XmlDocument xml = new XmlDocument();
			if(data.Length!=0) xml.LoadXml(data);
			PlayerDataRoot.LoadDocument(xml);
			transform.GetChild(0).gameObject.SetActive(true);
		}

		string MemoryToSerialized() {
			XmlDocument xml = new XmlDocument();
			PlayerDataRoot.SaveDocument(xml);
			return xml.InnerXml;
		}

		string EditorPath { get { return Application.dataPath+"/"+fileName; } }

	}

}