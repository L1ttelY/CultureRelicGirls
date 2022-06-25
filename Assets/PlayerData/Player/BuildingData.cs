using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData {

	public class BuildingData:DataBase {

		public const int levelNotUnlocked = -1;
		public const int levelNotBuilt = 0;

		public const int levelUpNot = 0;
		public const int levelUpYes = 1;

		public BuildingData(string name,DataBase parent) : base(name,parent) {
			level=new DataInt("level",this);
			levelUpStatus=new DataInt("levelUpStatus",this);
			levelUpProgression=new Progression("levelUpProgression",this);
			extraStatus=new DataInt("extraStatus",this);
			extraProgression=new Progression("extraProgression",this);
		}

		public DataInt level;
		public DataInt levelUpStatus;
		public Progression levelUpProgression;
		public DataInt extraStatus;
		public Progression extraProgression;

	}

}