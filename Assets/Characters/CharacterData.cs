using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	levels[0].levelUpCostTime为孵化时间
	levels[0].levelUpCost为孵化消耗素材量
 */

[CreateAssetMenu(menuName = "自定/角色数值")]
public class CharacterData:ScriptableObject {

	public static void ClearInstances() => datas.Clear();

	public static Dictionary<string,CharacterData> datas = new Dictionary<string,CharacterData>();
	private void OnEnable() {
		if(datas.ContainsKey(name)) return;
		datas.Add(name,this);
		for(int i = 0;i<levels.Length;i++) levels[i].parent=this;
	}

	[field: TextArea] [field: SerializeField] public string descriptionShort { get; private set; } //在选择人物后升级界面所展示的介绍
	[field: TextArea] [field: SerializeField] public string skillDescription { get; private set; } //角色的技能介绍
	[field: TextArea] [field: SerializeField] public string fullDescription { get; private set; }  //角色的背景介绍

	[field: SerializeField] public int maxLevel { get; private set; }
	[field: SerializeField] public GameObject combatPrefab { get; private set; }
	[field: SerializeField] public float healCostPerHp { get; private set; }
	[field: SerializeField] public float healTimPerHpInSecond { get; private set; }
	[field: SerializeField] public Sprite sprite { get; private set; } //战斗中的精灵
	[field: SerializeField] public Sprite portrait { get; private set; }//实物图片
	[field: SerializeField] public Sprite picture { get; private set; }//实物图片

	public CharacterLevelData[] levels;

}

[System.Serializable]
public struct CharacterLevelData {
	/// <summary>
	/// 升级消耗意识物质的量
	/// </summary>
	public int levelUpCost;
	/// <summary>
	/// 升级消耗的时间
	/// </summary>
	public System.TimeSpan levelUpCostTime { get { return new System.TimeSpan(levelUpCostHour,levelUpCostMinute,0); } }
	[HideInInspector] public int levelUpCostHour;
	[HideInInspector] public int levelUpCostMinute;
	[HideInInspector] public System.TimeSpan levelUpTimeTime { get { return new System.TimeSpan(levelUpTimeHour,levelUpTimeMinute,0); } }
	[HideInInspector] public int levelUpTimeHour;
	[HideInInspector] public int levelUpTimeMinute;

	public int hpMax;
	public int power;

	[HideInInspector] public CharacterData parent;
}