using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System.Xml;

namespace PlayerData {

	/// <summary>
	/// 存档的根节点
	/// </summary>
	public class PlayerData:DataBase {

		public static PlayerData instance { get; private set; }

		public PlayerData(string name,DataBase parent) : base(name,parent) {
			testProgression=new Progression("testProgression",this);
			sentienceMatter=new DataInt("sentienceMatter",this);
			printingMaterial=new DataInt("printingMaterial",this);
		}

		public static void LoadDocument(XmlDocument xml) {

			instance=new PlayerData("playerData",null);

			if(xml.DocumentElement==null) {
				//空
				instance.Load(null);
			} else {
				//读取数据
				XmlElement root = xml.DocumentElement;
				Debug.Log(root.Name);
				instance.Load(root);
			}
			Debug.Log(instance.testProgression.timeStart);
		}

		public static void SaveDocument(XmlDocument xml) {
			XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0","UTF-8","");
			xml.AppendChild(declaration);
			XmlElement root = xml.CreateElement("root");
			xml.AppendChild(root);
			//XmlElement playerData = xml.CreateElement("playerData");
			//root.AppendChild(playerData);

			instance.Save(root);
		}

		public DataInt sentienceMatter;
		public DataInt printingMaterial;

		Progression testProgression;

	}

}