using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Combat {

	public class CombatRewardUIController:MonoBehaviour {

		[SerializeField] GameObject victory;
		[SerializeField] GameObject fail;
		[SerializeField] Image rewardCharacterSmall;
		[SerializeField] Image rewardCharacterBig;
		[SerializeField] Text rewardSmDisplay;
		[SerializeField] Text rewardPmDisplay;
		[SerializeField] Text rewardCharacterText;
		[SerializeField] AnimationCurve popUpCurve;
		[SerializeField] AudioClip soundNewCharacter;

		public static CombatRewardUIController instance { get; private set; }

		bool usePopup;
		float timeAfterPopup;
		float timeAfterActivate;

		public void Init() {
			instance=this;
		}

		private void Update() {

			timeAfterActivate+=Time.deltaTime;

			if(timeAfterActivate>1&&Input.GetMouseButtonDown(0)) {

				if(!usePopup) SceneManager.LoadScene("Museum");
				else {
					AudioController.PlayAudio(soundNewCharacter,Camera.main.transform.position);
					rewardCharacterText.text=$"已获得文物\n{CharacterData.datas[rewardCharacterIndex].name}";
					GameObject popUpObject = rewardCharacterBig.transform.parent.gameObject;
					popUpObject.SetActive(true);
				}
			}

			if(rewardCharacterBig.gameObject.activeInHierarchy) {
				timeAfterPopup+=Time.deltaTime;
				if(timeAfterPopup<1) rewardCharacterBig.transform.parent.localScale=popUpCurve.Evaluate(timeAfterPopup)*Vector3.one;
				if(timeAfterPopup>1) usePopup=false;
			}

		}

		int rewardCharacterIndex;

		public void EnterMode(bool isVictory,int rewardSm,int rewardPm,int rewardCharacterIndex) {
			gameObject.SetActive(true);
			usePopup=false;
			rewardSmDisplay.text=rewardSm.ToString();
			if(isVictory) {

				victory.SetActive(true);
				rewardPmDisplay.text=rewardPm.ToString();
				if(rewardCharacterIndex>0) {
					this.rewardCharacterIndex=rewardCharacterIndex;
					rewardCharacterBig.sprite=CharacterData.datas[rewardCharacterIndex].picture;
					rewardCharacterSmall.sprite=CharacterData.datas[rewardCharacterIndex].picture;
					usePopup=true;
				} else {
					rewardCharacterSmall.color=Color.clear;
				}

			} else {
				fail.SetActive(true);
			}

		}

	}

}