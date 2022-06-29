using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	levels[0].levelUpCostTimeΪ����ʱ��
	levels[0].levelUpCostΪ���������ز���
 */

[CreateAssetMenu(menuName = "�Զ�/��ɫ��ֵ")]
public class CharacterData:ScriptableObject {

	public static Dictionary<int,CharacterData> datas = new Dictionary<int,CharacterData>();
	private void OnEnable() {
		datas.Add(id,this);
	}

	[field: TextArea] [field: SerializeField] public string descriptionShort { get; private set; } //��ѡ�����������������չʾ�Ľ���
	[field: TextArea] [field: SerializeField] public string skillDescription { get; private set; } //��ɫ�ļ��ܽ���
	[field: TextArea] [field: SerializeField] public string fullDescription { get; private set; }  //��ɫ�ı�������

	[field: SerializeField] public int id { get; private set; }
	[field: SerializeField] public int maxLevel { get; private set; }
	[field: SerializeField] public GameObject combatPrefab { get; private set; }
	[field: SerializeField] public float healCostPerHp { get; private set; }
	[field: SerializeField] public float healTimPerHpInSecond { get; private set; }
	[field: SerializeField] public Sprite sprite { get; private set; } //ս���еľ���
	[field: SerializeField] public Sprite picture { get; private set; }//ʵ��ͼƬ

	public CharacterLevelData[] levels;

}

[System.Serializable]
public struct CharacterLevelData {
	/// <summary>
	/// ����������ʶ���ʵ���
	/// </summary>
	public int levelUpCost;
	/// <summary>
	/// �������ĵ�ʱ��
	/// </summary>
	public System.TimeSpan levelUpCostTime { get { return new System.TimeSpan(levelUpCostHour,levelUpCostMinute,0); } }
	public int levelUpCostHour;
	public int levelUpCostMinute;
	public System.TimeSpan levelUpTimeTime { get { return new System.TimeSpan(levelUpTimeHour,levelUpTimeMinute,0); } }
	public int levelUpTimeHour;
	public int levelUpTimeMinute;

	public int hpMax;
	public int power;
}