using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	/// <summary>
	/// UI界面的基类
	/// 要创建新的UI界面请继承此类
	/// 用重载的Init来做初始化函数 而不是Start
	/// </summary>
	public class UIModeBase:MonoBehaviour {

		virtual public void Init() {
			UIController.instance.AddMode(gameObject.name,this);
		}

	}
}