using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	[System.Serializable]
	class FlagUse {
		public GameObject targetGameObject;
		public MonoBehaviour targetScript;
		[Tooltip("��ΪTrue ����Flag��ʱ��Ӧ���忪 ��ΪFalse ����Flag��ʱ��Ӧ�����")]
		public bool activeState;
	}

	public class FlagUser:MonoBehaviour {

		[Tooltip("Flag���� �����������Զ�����")]
		[SerializeField] string flagName;
		[Tooltip("�Զ���������ʱ��ʹ�õĶ��� ������ʹ���Լ�")]
		[SerializeField] GameObject autoNameFrom;
		[Tooltip("ʹ�õĶ���")]
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
