using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerData {

  public class CharacterData:DataBase {

    public const int levelNotFound = -1;
    public const int levelNotUnlcoked = 0;

    public const int levelUpNot = 0;
    public const int levelUpCost = 1;
    public const int levelUpTime = 2;

    public const int healNot = 0;
    public const int healCost = 1;
    public const int healTime = 2;

    public float healthAmount {
      get { return (float)healthAmountOver100000000.value/(float)100000000; }
      set { healthAmountOver100000000.value=(int)((float)100000000*value); }
    }

    public CharacterData(string name,DataBase parent) : base(name,parent) {
      level=new DataInt("level",this);
      levelUpStatus=new DataInt("levelUpStatus",this);
      healStatus=new DataInt("healStatus",this);
      levelUpProgression=new Progression("levelUpProgression",this);
      healProgression=new Progression("healProgression",this);
      healthAmountOver100000000=new DataInt("healthAmountOver100000000",this);
    }

    public DataInt level;

    public DataInt healthAmountOver100000000;
    public DataInt levelUpStatus;
    public DataInt healStatus;

    public Progression levelUpProgression;
    public Progression healProgression;


  }

}