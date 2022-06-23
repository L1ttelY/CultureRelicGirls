using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "自定/角色数值")]
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
	/// 升级消耗材料的量
	/// </summary>
	public int levelUpCostMaterial;
	/// <summary>
	/// 升级消耗意识物质的量
	/// </summary>
	public int levelUpCostSentienceMatter;
	/// <summary>
	/// 升级消耗的时间
	/// </summary>
	public System.TimeSpan levelUpTime { get { return new System.TimeSpan(levelUpHour,levelUpMinute,0); } }
	public int levelUpHour;
	public int levelUpMinute;

	public int hpMax;
	public int power;

}