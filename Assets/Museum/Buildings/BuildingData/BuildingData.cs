using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	/// <summary>
	/// ÿ���������Ӧ����ֵ
	/// �زĵ����Ƽ�Ϊ�����������
	/// </summary>
	[CreateAssetMenu(menuName = "�Զ�/������ֵ")]
	public class BuildingData:ScriptableObject {

		//public static List<BuildingData> datas = new List<BuildingData>();
		private void OnEnable() {
			//datas.Add(this);
		}

		public BuildingLevelData[] levels;
		[field: SerializeField] public int maxLevel { get; private set; }

	}


	/// <summary>
	/// ÿ����������ȼ��仯����ֵ
	/// </summary>
	[System.Serializable]
	public struct BuildingLevelData {

		/// <summary>
		/// �������Ĳ��ϵ���
		/// </summary>
		public int levelUpCostMaterial;
		/// <summary>
		/// �������ĵ�ʱ��
		/// </summary>
		public System.TimeSpan levelUpTime { get { return new System.TimeSpan(levelUpHour,levelUpMinute,0); } }
		public int levelUpHour;
		public int levelUpMinute;
		[field: SerializeField] [field: TextArea] public string description { get; private set; }
		[field: SerializeField] [field: TextArea] public string levelUpMessage { get; private set; }

	}

}