using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class CountDownController:MonoBehaviour {

		public class CountDownToken {
			public Text textField;
			public GameObject boundObject;
			public Image progressionImage;
		}

		public static CountDownController instance { get; private set; }
		private void Start() {
			instance=this;
		}

		[SerializeField] GameObject countDownPrefab;

		public CountDownToken CreateCountDown() {
			CountDownToken result = new CountDownToken();
			result.boundObject=Instantiate(countDownPrefab,transform);
			result.textField=result.boundObject.GetComponentInChildren<Text>();
			result.progressionImage=result.boundObject.transform.GetChild(0).GetComponent<Image>();
			return result;
		}

		public void FreeCountDown(CountDownToken token) {
			if(token==null) return;
			Destroy(token.boundObject);
		}

	}

}