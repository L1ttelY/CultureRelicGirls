using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	[CreateAssetMenu(menuName = "×Ô¶¨/¾çÇéÆ¬¶Î")]
	public class StoryData:ScriptableObject {
		public string title;
		[TextArea(10,10000)] public string text;
	}

}