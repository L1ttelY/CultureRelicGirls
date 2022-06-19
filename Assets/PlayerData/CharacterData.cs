using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData {

	/// <summary>
	/// 存档的角色信息节点
	/// </summary>
	public class CharacterData:DataBase {

		public const int levelNotFound = -1;
		public const int levelNotUnlcoked = 0;

		public const int levelUpNot = 0;
		public const int levelUpCost = 1;
		public const int levelUpTime = 2;

		public const int healNot = 0;
		public const int healCost = 1;
		public const int healTime = 2;

		public CharacterData(string name,DataBase parent) : base(name,parent) {
			level=new DataInt("level",this);
			levelUpStatus=new DataInt("levelUpStatus",this);
			healStatus=new DataInt("healStatus",this);
			levelUpProgression=new Progression("levelUpProgression",this);
			healProgression=new Progression("healProgression",this);
		}

		public DataInt level;

		public DataInt levelUpStatus;
		public DataInt healStatus;

		public Progression levelUpProgression;
		public Progression healProgression;

	}

}