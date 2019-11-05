using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New building CFG", menuName = "Engine/Entities/Spawners/New Building Configuration")]
public class BuildingConfiguration : ScriptableObject {

    [SerializeField]
    private List<SpawnerConfig> tiers;
    public List<SpawnerConfig> Tiers { get { return tiers; } }

}
