using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[System.Serializable]
	class FlagUse {
		public GameObject targetGameObject;
		public MonoBehaviour targetScript;
		[Tooltip("若为True 则在Flag开时对应物体开 若为False 则在Flag关时对应物体关")]
		public bool activeState;
	}

	public class FlagUser:MonoBehaviour {

		[Tooltip("Flag名称 留空以设置自动名称")]
		[SerializeField] string flagName;
		[Tooltip("自动设置名称时所使用的对象 留空以使用自己")]
		[SerializeField] GameObject autoNameFrom;
		[Tooltip("使用的对象")]
		[SerializeField] FlagUse[] uses;
		[SerializeField]

		private void Start() {
			if(autoNameFrom==null) autoNameFrom=gameObject;
			if(flagName.Length==0) flagName=Utility.GenerateNameFromGameObject(autoNameFrom);
		}

		private void Update() {

			bool flagSet = PlayerData.Flags.instance.HasFlag(flagName);

			foreach(var i in uses) {
				if(i.targetGameObject) i.targetGameObject.SetActive(flagSet==i.activeState);
				if(i.targetScript) i.targetScript.enabled=(flagSet==i.activeState);
			}
		}

	}

}
