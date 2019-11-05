using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Gatherable Stats", menuName = "Engine/Entities/New Entity Gatherable Stats")]
public class EntityGatherableStats : ScriptableObject {

    [SerializeField]
    private int units = 1000;
    [SerializeField]
    private int valuePerUnit = 1;
    [SerializeField]
    private float unitGatherTime = 1;

    public int Units { get { return units; } }
    public int ValuePerUnit { get { return valuePerUnit; } }
    public float UnitGatherTime { get { return unitGatherTime; } }

}
