using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "�Զ�/��ɫ��ֵ")]
public class CharacterData:ScriptableObject {

	public static Dictionary<int,CharacterData> datas;
	private void OnEnable() {
		datas.Add(id,this);
	}

	[field: SerializeField] public int id { get; private set; }
	[field: SerializeField] public int maxLevel { get; private set; }
	[field: SerializeField] public GameObject combatPrefab { get; private set; }
	public CharacterLevelData[] levels;

}

[System.Serializable]
public struct CharacterLevelData {
	/// <summary>
	/// �������Ĳ��ϵ���
	/// </summary>
	public int levelUpCostMaterial;
	/// <summary>
	/// ����������ʶ���ʵ���
	/// </summary>
	public int levelUpCostSentienceMatter;
	/// <summary>
	/// �������ĵ�ʱ��
	/// </summary>
	public System.TimeSpan levelUpTime { get { return new System.TimeSpan(levelUpHour,levelUpMinute,0); } }
	public int levelUpHour;
	public int levelUpMinute;

	public int hpMax;
	public int power;

}