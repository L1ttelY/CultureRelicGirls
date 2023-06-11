using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "�Զ�/վ̨��Ϣ")]
public class StationData:CombatEntry {

	[field: SerializeField] public string regionName { get; private set; }
	[field: SerializeField] public string stationName { get; private set; }

	public static void ClearInstances() {
		instances.Clear();
	}

	public static readonly HashSet<StationData> instances = new HashSet<StationData>();

	private void OnEnable() {
		instances.Add(this);
	}
}
