using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnableCFG", menuName = "Engine/Entities/Spawners/New SpawnableCFG")]
public class SpawnableConfig : ScriptableObject {

    [SerializeField]
    private EntityUiData uiData;
    [SerializeField]
    private int cost;
    [SerializeField]
    private float duration;
    [SerializeField]
    private GameObject prefab;

    public EntityUiData UiData { get { return uiData; } }
    public int Cost { get { return cost; } }
    public GameObject Prefab { get { return prefab; } }
    public float Duration { get { return duration; } }

}
