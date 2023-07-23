using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public static class LoadoutController {

	public static DataString[] teamMembers = new DataString[3];
	public static DataString[] hotBar = new DataString[5];
	public static DataBase loadoutRoot;

	public static CharacterData GetTeamMember(int index) {
		if(!CharacterData.datas.ContainsKey(teamMembers[index].value)) return null;
		return CharacterData.datas[teamMembers[index].value];
	}
	public static void SetTeamMember(int index,CharacterData value) {
		if(value==null) teamMembers[index].value="";
		else teamMembers[index].value=value.name;
	}
	public static ItemData GetHotBar(int index) {
		if(!ItemData.instances.ContainsKey(hotBar[index].value)) return null;
		return ItemData.instances[hotBar[index].value];
	}
	public static void SetHotBar(int index,ItemData value) => hotBar[index].value=value.name;

	[RuntimeInitializeOnLoadMethod]
	static void Init() {
		PlayerDataRoot.OnRootCreation+=PlayerDataRoot_OnRootCreation;
	}

	private static void PlayerDataRoot_OnRootCreation(object sender) {
		DataBase root = sender as DataBase;
		loadoutRoot=new DataBase("loadoutRoot",root);
		for(int i = 0;i<3;i++) teamMembers[i]=new DataString($"teamMember{i}",loadoutRoot);
		for(int i = 0;i<5;i++) hotBar[i]=new DataString($"hotBar{i}",loadoutRoot);

	}
}