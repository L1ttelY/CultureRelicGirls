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

		public float progressionAmount {
			get {
				System.TimeSpan spanTotal = timeEnd-timeStart;
				System.TimeSpan spanNow = System.DateTime.Now-timeStart;
				float progressionAmount = (float)(spanNow/spanTotal);
				if(progressionAmount<0) progressionAmount=0;
				if(progressionAmount>1) progressionAmount=1;
				return progressionAmount;
			}
		}

		public bool completion {
			get {
				return System.DateTime.Now>=timeEnd;
			}
		}

		public void SetProgression(System.TimeSpan timeTotal,float progressionNow) {
			System.DateTime now = System.DateTime.Now;
			timeStart=now-timeTotal*progressionNow;
			timeEnd=now+timeTotal*(1-progressionNow);

		}


	}
}