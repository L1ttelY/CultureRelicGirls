using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

namespace PlayerData {
	public class DataBase {

		public Dictionary<string,DataBase> children;
		public string name;

		public DataBase(string name,DataBase parent) {
			children=new Dictionary<string,DataBase>();
			this.name=name;
			if(parent!=null) parent.AddChild(this);
		}

		public virtual void Load(XmlElement serialized) {
			if(serialized==null){
				foreach(var child in children) child.Value.Load(null);
				return;
			}

			Dictionary<string,bool> loaded = new Dictionary<string,bool>();
			foreach(var child in children) loaded.Add(child.Key,false);

			foreach(XmlNode serializedChild in serialized.ChildNodes) {
				foreach(var child in children) {

					if(serializedChild.Name==child.Key) {
						child.Value.Load(serializedChild as XmlElement);	
						loaded[child.Key]=true;
					}
				}
			}

			foreach(var child in children) {
				if(!loaded[child.Key]) {
					child.Value.Load(null);
				}
			}

		}

		public virtual void Save(XmlElement target) {
			foreach(var child in children) {
				XmlElement newElement = target.OwnerDocument.CreateElement(child.Key);
				child.Value.Save(newElement);
				target.AppendChild(newElement);
			}
		}

		public void AddChild(DataBase child) {
			children.Add(child.name,child);
		}
	}
}
