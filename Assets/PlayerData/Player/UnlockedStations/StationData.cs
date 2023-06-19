using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "自定/站台信息")]
public class StationData:CombatEntry, System.IComparable<StationData> {

	[field: SerializeField] public string regionName { get; private set; }
	[field: SerializeField] public string stationName { get; private set; }
	[field: SerializeField] public Sprite thumbnail { get; private set; }

	public static void ClearInstances() {
		instances.Clear();
	}

	public static readonly Dictionary<string,StationData> instances = new Dictionary<string, StationData>();

	private void OnEnable() {
		if(instances.ContainsKey(name)) return;
		instances.Add(name,this);
	}

	public int CompareTo(StationData obj) {
		int regionComparison = regionName.CompareTo(obj.regionName);
		if(regionComparison!=0) return regionComparison;
		return stationName.CompareTo(obj.stationName);
	}

	public static bool operator >(StationData operand1,StationData operand2) {
		return operand1.CompareTo(operand2)>0;
	}
	public static bool operator <(StationData operand1,StationData operand2) {
		return operand1.CompareTo(operand2)<0;
	}
	public static bool operator >=(StationData operand1,StationData operand2) {
		return operand1.CompareTo(operand2)>=0;
	}
	public static bool operator <=(StationData operand1,StationData operand2) {
		return operand1.CompareTo(operand2)<=0;
	}

}
