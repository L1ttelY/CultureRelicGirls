using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat {

	public class FlagSetter:MonoBehaviour {

		[Tooltip("Flag���� �����������Զ�����")]
		[SerializeField] string flagName;
		[Tooltip("�Զ���������ʱ��ʹ�õĶ��� ������ʹ���Լ�")]
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