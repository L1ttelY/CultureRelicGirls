using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInformationDisplay:MonoBehaviour {

	[SerializeField] Text textName;
	[SerializeField] Text textClass;
	[SerializeField] Text textLevel;
	[SerializeField] Text textHp;
	[SerializeField] Text textPower;
	[SerializeField] Text textNextLevel;
	[SerializeField] Text textNextHp;
	[SerializeField] Text textNextPower;
	[SerializeField] Text textSkillName;
	[SerializeField] Text textSkillDescription;
	[SerializeField] Text textSkillDetail;
	[SerializeField] Text textNextSkillDetail;
	[SerializeField] Text textRelic;
	[SerializeField] Image characterImage;
	[SerializeField] Image relicImage;

	public CharacterData targetCharacter;

	string[] numberList = { "零","一","二","三","四","五","六","七","八","九","十" };

	void Display(Text target,string content) {
		if(!target) return;
		target.text=content;
	}

	private void OnEnable() {
		Update();
	}

	private void Update() {

		if(targetCharacter) {

			int currentLevel = PlayerData.CharacterDataRoot.instance.characters[targetCharacter.name].level.value;
			CharacterLevelData currentData = targetCharacter.levels[currentLevel];

			Display(textName,targetCharacter.name);
			Display(textClass,targetCharacter.className);
			Display(textSkillName,targetCharacter.skillName);
			Display(textSkillDescription,targetCharacter.skillDescription);
			Display(textRelic,targetCharacter.relicDescr);
			Display(textLevel,numberList[currentLevel]);
			Display(textHp,currentData.hpMax.ToString());
			Display(textPower,currentData.power.ToString());
			Display(textSkillDetail,currentData.skillDetail);

			if(characterImage) {
				characterImage.sprite=targetCharacter.portrait;
				characterImage.color=Color.white;
			}
			if(relicImage) {
				relicImage.sprite=targetCharacter.relicPicture;
				relicImage.color=Color.white;
			}

			if(currentLevel<targetCharacter.levels.Length-1) {
				CharacterLevelData nextData = targetCharacter.levels[currentLevel+1];

				Display(textNextLevel,numberList[currentLevel+1]);
				Display(textNextHp,nextData.hpMax.ToString());
				Display(textNextPower,nextData.power.ToString());
				Display(textNextSkillDetail,targetCharacter.name);

			} else {

				Display(textNextLevel,"");
				Display(textNextHp,"");
				Display(textNextPower,"");
				Display(textNextSkillDetail,"");

			}
		} else {

			Display(textName,"");
			Display(textClass,"");
			Display(textSkillName,"");
			Display(textSkillDescription,"");
			Display(textLevel,"");
			Display(textHp,"");
			Display(textPower,"");
			Display(textSkillDetail,"");
			Display(textNextLevel,"");
			Display(textNextHp,"");
			Display(textNextPower,"");
			Display(textNextSkillDetail,"");

			if(characterImage) {
				characterImage.color=Color.clear;
			}
			if(relicImage) {
				relicImage.color=Color.clear;
			}
		}

	}

}
