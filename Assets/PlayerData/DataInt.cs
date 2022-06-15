using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public class DataInt:DataBase {

		public int value;

		public DataInt(string name,DataBase parent) : base(name,parent) { }
		public override void Load(XmlElement serialized) {
			base.Load(serialized);
			if(serialized!=null) value=int.Parse(serialized.InnerText);
			else value=0;	
		}
		public override void Save(XmlElement target) {
			base.Save(target);
			target.InnerText=value.ToString();
		}

	}
}