using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData {

	public class EnemyData:DataBase {

		public EnemyData(string name,DataBase parent) : base(name,parent) {
			x=new DataFloat("x",this);
			enemyType=new DataInt("enemyType",this);
		}

		public DataFloat x;
		public DataInt enemyType;

	}

}