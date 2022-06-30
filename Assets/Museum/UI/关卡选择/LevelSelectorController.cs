using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Museum {

	public class LevelSelectorController:MonoBehaviour {

		readonly static Color gray = new Color(0.4f,0.4f,0.4f);

		[SerializeField] int levelId;
		[SerializeField] Image pathImage;
		[SerializeField] Image latestImage;
		[SerializeField] Sprite spriteLocked;
		Sprite spriteUnlocked;
		Image image;

		private void Start() {
			image=GetComponent<Image>();
			spriteUnlocked=image.sprite;
		}

		bool unlocked;
		private void Update() {
			latestImage.gameObject.SetActive(PlayerData.PlayerDataRoot.instance.campaignProgression.value==levelId);
			if(PlayerData.PlayerDataRoot.instance.campaignProgression.value>=levelId) {
				//�ѽ���
				if(pathImage) pathImage.color=Color.white;
				image.sprite=spriteUnlocked;
				unlocked=true;
			} else {
				//δ����
				if(pathImage) pathImage.color=gray;
				image.sprite=spriteLocked;
				unlocked=false;
			}
		}

		public void OnClick() {
			if(!unlocked) {
				string message =
					"����ؿ���û�н���\n"+
					"�����ǻ���Լ�����Ϸ";
				ConfirmationMode.EnterMode(message,LevelSelectionMode.EnterMode,LevelSelectionMode.EnterMode);
				return;
			}



			string levelName = $"Level{levelId}.xml";
			LevelStartMode.EnterMode(true,levelName,levelId);

		}

	}

}