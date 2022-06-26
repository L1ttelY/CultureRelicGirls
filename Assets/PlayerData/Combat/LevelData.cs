using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

namespace PlayerData {

	public class LevelData:DataBase {

		public void LoadFile(string path) {
			string data = "";
			if(File.Exists(path)) data=File.ReadAllText(path);
			XmlDocument xml = new XmlDocument();
			if(data.Length!=0) xml.LoadXml(data);
			Load(xml.DocumentElement);
		}

		public void SaveFile(string path) {
			XmlDocument xml = new XmlDocument();
			XmlDeclaration declaration = xml.CreateXmlDeclaration("1.0","UTF-8","");
			xml.AppendChild(declaration);
			XmlElement root = xml.CreateElement("root");
			xml.AppendChild(root);
			Save(root);
			if(!File.Exists(path)) File.Create(path).Dispose();
			File.WriteAllText(path,xml.InnerXml);
		}

		public LevelData() : base("LevelData",null) {

			sceneName=new DataString("sceneName",this);

			rewardSm=new DataInt("rewardSm",this);
			rewardPm=new DataInt("rewardPm",this);
			rewardCharacter=new DataInt("rewardCharacter",this);
			startX=new DataFloat("startX",this);
			endX=new DataFloat("endX",this);

			enemyCount=new DataInt("enemyCount",this);
			enemies=new EnemyData[400];
			for(int i = 0;i<400;i++) {
				enemies[i]=new EnemyData($"enemy{i}",this);
			}
		}

		public DataString sceneName;

		public DataInt rewardSm;
		public DataInt rewardPm;
		public DataInt rewardCharacter;
		
		public DataFloat startX;
		public DataFloat endX;

		public DataInt enemyCount;
		public EnemyData[] enemies;

	}

}