using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public interface IDataModel {
		public void Load(XmlElement xml);
		public void Save(XmlElement xml);
	}
}