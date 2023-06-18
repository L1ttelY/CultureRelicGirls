using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class FlagSetter:MonoBehaviour {

		[Tooltip("Flag名称 留空以设置自动名称")]
		[SerializeField] string flagName;
		[Tooltip("自动设置名称时所使用的对象 留空以使用自己")]
		[SerializeField] GameObject autoNameFrom;

		private void Start() {
			if(autoNameFrom==null) autoNameFrom=gameObject;
			if(flagName.Length==0) flagName=Utility.GenerateNameFromGameObject(autoNameFrom);
		}

		public void SetFlag() {
			PlayerData.Flags.instance.SetFlag(flagName);
		}

	}

}