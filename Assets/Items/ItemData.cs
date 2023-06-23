using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "自定/道具")]
public class ItemData:ScriptableObject {

	[field: SerializeField] string className { get; set; }
	[field: SerializeField] public Sprite sprite { get; private set; }
	[SerializeField] int countTotal;

	public static Dictionary<ItemData,int> countLeft=new Dictionary<ItemData, int>();
	public static void ResetCount() {
		foreach(var i in instances) {
			countLeft[i.Value]=i.Value.countTotal;
		}

	}

	System.Type boundType;
	ItemBase boundItem;

	public static readonly Dictionary<string,ItemData> instances = new Dictionary<string,ItemData>();

	void OnEnable() {

		countLeft[this]=countTotal;
		instances.Add(name,this);

		boundType=System.Type.GetType(className);
		boundItem=boundType.GetConstructor(new System.Type[0]).Invoke(new object[0]) as ItemBase;

	}

	public void InvokeUse() {

		if(countLeft[this]<=0) return;

		try {
			boundItem.Use();
			countLeft[this]--;
		} catch(System.Exception) { }
	}

	void Unload() {

	}

	public static void ClearInstances() {
		foreach(var i in instances) i.Value.Unload();
		instances.Clear();
	}

}