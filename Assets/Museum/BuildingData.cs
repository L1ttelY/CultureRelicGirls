using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Museum {

	/// <summary>
	/// ÿ���������Ӧ����ֵ
	/// </summary>
	[CreateAssetMenu(menuName = "�Զ�/������ֵ")]
	public class BuildingData:ScriptableObject {

		public static Dictionary<int,BuildingData> datas;
		private void OnEnable() {
			datas.Add(id,this);
		}

		public BuildingLevelData[] levels;
		[field: SerializeField] public int maxLevel { get; private set; }
		[field: SerializeField] public int id { get; private set; }

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
		/// ����������ʶ���ʵ���
		/// </summary>
		public int levelUpCostSentienceMatter;
		/// <summary>
		/// �������ĵ�ʱ��
		/// </summary>
		public System.TimeSpan levelUpTime { get { return new System.TimeSpan(levelUpHour,levelUpMinute,0); } }
		public int levelUpHour;
		public int levelUpMinute;

	}

}