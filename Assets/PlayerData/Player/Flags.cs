using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PlayerData {

	public class Flags:DataString {

		public static Flags instance { get; private set; }

		public bool HasFlag(string name) => loadedFlags.Contains(name);
		public void SetFlag(string name) {
			if(loadedFlags.Contains(name)) return;
			loadedFlags.Add(name);
			value+=$";==;{name}";
		}
		public void RemoveFlag(string name) {
			if(!loadedFlags.Contains(name)) return;
			loadedFlags.Remove(name);
			System.Text.StringBuilder newValue = new System.Text.StringBuilder();
			foreach(var i in loadedFlags) newValue.Append($";==;{i}");
			value=newValue.ToString();
		}

		HashSet<string> loadedFlags = new HashSet<string>();


		public Flags(string name,DataBase parent) : base(name,parent) {
			instance=this;
		}

		public override void Load(XmlElement serialized) {
			base.Load(serialized);
			loadedFlags.Clear();
			string[] flags = value.Split(";==;");
			if(flags==null) flags=new string[0];
			foreach(var i in flags) loadedFlags.Add(i);
		}

	}

}