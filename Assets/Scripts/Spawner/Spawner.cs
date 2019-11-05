using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour {

    [SerializeField]
    protected GamePoint gamePoint;

    public abstract void Spawn(SpawnableConfig entityPrefab, int team);

}
