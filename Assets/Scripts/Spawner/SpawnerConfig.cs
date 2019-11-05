using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnCFG", menuName = "Engine/Entities/Spawners/New Spawner Configuration")]
public class SpawnerConfig : ScriptableObject {

    [SerializeField]
    private List<SpawnableConfig> items;
    public List<SpawnableConfig> Items { get { return items; } }

}
