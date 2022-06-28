using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	[CreateAssetMenu(menuName = "�Զ�/����Ƭ��")]
	public class StoryData:ScriptableObject {
		public string title;
		[TextArea] public string text;
	}

}