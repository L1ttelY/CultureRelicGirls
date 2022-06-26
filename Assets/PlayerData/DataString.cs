using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public class DataString:DataBase {

		public string value;

		public DataString(string name,DataBase parent) : base(name,parent) { }
		public override void Load(XmlElement serialized) {
			base.Load(serialized);
			if(serialized!=null) value=serialized.InnerText;
			else value="";
		}
		public override void Save(XmlElement target) {
			base.Save(target);
			target.InnerText=value;
		}

	}
}