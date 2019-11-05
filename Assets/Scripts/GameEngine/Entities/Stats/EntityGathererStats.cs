using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Gatherer Stats", menuName = "Engine/Entities/New Entity Gatherer Stats")]
public class EntityGathererStats : ScriptableObject {

    [SerializeField]
    private float gatherSpeed = 1;
    [SerializeField]
    private float gatherRange = 1;

    public float GatherSpeed { get { return gatherSpeed; } }
    public float GatherRange { get { return gatherRange; } }
}
