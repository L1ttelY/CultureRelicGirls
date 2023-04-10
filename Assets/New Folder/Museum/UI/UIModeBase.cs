using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	/// <summary>
	/// UI����Ļ���
	/// Ҫ�����µ�UI������̳д���
	/// �����ص�Init������ʼ������ ������Start
	/// </summary>
	public class UIModeBase:MonoBehaviour {

		public virtual bool OverrideQuitButton() {
			return false;
		}

		virtual public void Init() {
			UIController.instance.AddMode(gameObject.name,this);
		}

	}
}