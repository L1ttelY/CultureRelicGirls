using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackStateTransistionType {
	Move,
	Attack
}

[System.Serializable]
public class AttackStateTransistion {

	[field: SerializeField] public AttackStateTransistionType type { get; private set; }
	[field: SerializeField] public int attackId { get; private set; }
	[field: SerializeField] public float weight { get; private set; }

}


[System.Serializable]
public class AttackStateData {

	[field: SerializeField] public float minDistance { get; private set; }
	[field: SerializeField] public float maxDistance { get; private set; }
	[field: SerializeField] public float startWeight { get; private set; }

	[HideInInspector] public int id;

	public List<AttackStateTransistion> transitionList;

}