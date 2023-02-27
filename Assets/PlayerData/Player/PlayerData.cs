using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;
using System.Xml;

namespace PlayerData {

	/// <summary>
	/// 存档的根节点
	/// </summary>
	public class PlayerDataRoot:DataBase {

		public static int smCount { get { return instance.sentienceMatter.value; } set { instance.sentienceMatter.value=value; } }
		public static int pmCount { get { return instance.printingMaterial.value; } set { instance.printingMaterial.value=value; } }

		public static PlayerDataRoot instance { get; private set; }

		public static event EventHandler OnRootCreation;

		public PlayerDataRoot(string name,DataBase parent) : base(name,parent) {
			sentienceMatter=new DataInt("sentienceMatter",this);
			printingMaterial=new DataInt("printingMaterial",this);

			characterDatas=new CharacterData[20];
			for(int i = 0;i<20;i++) characterDatas[i]=new CharacterData("character"+i,this);

			buildingDatas=new BuildingData[30];
			for(int i = 0;i<30;i++) buildingDatas[i]=new BuildingData("building"+i,this);

			storyStatus=new DataInt[30];
			for(int i = 0;i<30;i++) storyStatus[i]=new DataInt($"story{i}",this);
			storyPosition=new DataFloat[30];
			for(int i = 0;i<30;i++) storyPosition[i]=new DataFloat($"storyPosition{i}",this);

			campaignProgression=new DataInt("campaignProgression",this);

			OnRootCreation?.Invoke(this);

		}

		public static void LoadDocument(XmlDocument xml) {

			instance=new PlayerDataRoot("playerData",null);

			if(xml.DocumentElement==null) {
				//空
				instance.Load(null);
			} else {
				//读取数据
				XmlElement root = xml.DocumentElement;
				instance.Load(root);
			}

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

		public CharacterData[] characterDatas;
		public BuildingData[] buildingDatas;

		public DataInt campaignProgression;

		public const int storyLocked = 0;
		public const int storyUnlocked = 0;
		public const int storyViewed = 0;
		public DataInt[] storyStatus;
		public DataFloat[] storyPosition;


	}

}