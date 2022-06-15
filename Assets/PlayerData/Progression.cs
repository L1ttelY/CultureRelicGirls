using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public class Progression:DataBase {

		public System.DateTime timeStart;
		public System.DateTime timeEnd;

		public Progression(string name,DataBase parent) : base(name,parent) { }

		override public void Load(XmlElement xml) {
			if(xml==null) {
				timeStart=System.DateTime.UtcNow;
				timeEnd=timeStart.AddDays(1);
				Debug.Log("initialize : "+timeStart);
			} else {
				foreach(XmlNode i in xml.ChildNodes) {
					if(i.Name=="timeStartElement") timeStart=System.DateTime.Parse(i.InnerText);
					if(i.Name=="timeEndElement") timeEnd=System.DateTime.Parse(i.InnerText);
				}
			}
		}

		override public void Save(XmlElement xml) {
			XmlElement timeStartElement = xml.OwnerDocument.CreateElement("timeStartElement");
			XmlElement timeEndElement = xml.OwnerDocument.CreateElement("timeEndElement");
			timeStartElement.InnerText=timeStart.ToString();
			timeEndElement.InnerText=timeEnd.ToString();
			xml.AppendChild(timeStartElement);
			xml.AppendChild(timeEndElement);
		}
	}
}