using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public class DataBool:DataBase {

		public bool value;

		public DataBool(string name,DataBase parent) : base(name,parent) { }
		public override void Load(XmlElement serialized) {
			base.Load(serialized);
			if(serialized!=null) value=bool.Parse(serialized.InnerText);
			else value=false;
		}
		public override void Save(XmlElement target) {
			base.Save(target);
			target.InnerText=value.ToString();
		}

	}
}