using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System.Xml;

namespace PlayerData {

	/// <summary>
	/// 存档的根节点
	/// </summary>
	public static class PlayerData {

		public static void Load(XmlDocument xml) {

			if(xml.DocumentElement==null) {
				//空

			} else {
				//读取数据
				XmlElement root = xml.DocumentElement;
				foreach(XmlNode i in root.ChildNodes) {
					if(i.Name=="sentienceMatter") sentienceMatter=int.Parse(i.InnerText);
					if(i.Name=="printingMaterial") printingMaterial=int.Parse(i.InnerText);
				}

			}

			Debug.Log(sentienceMatter+" . "+printingMaterial);

		}

		public static void Save(XmlDocument xml) {
			XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0","UTF-8","");
			xml.AppendChild(declaration);

			XmlElement root = xml.CreateElement("root");
			xml.AppendChild(root);

			XmlElement sentienceMatterNode = xml.CreateElement("sentienceMatter");
			sentienceMatterNode.InnerText=sentienceMatter.ToString();
			XmlElement printingMaterialNode = xml.CreateElement("printingMaterial");
			printingMaterialNode.InnerText=printingMaterial.ToString();

			root.AppendChild(sentienceMatterNode);
			root.AppendChild(printingMaterialNode);


		}

		static public int sentienceMatter = 100;
		static public int printingMaterial = 100;

	}

}