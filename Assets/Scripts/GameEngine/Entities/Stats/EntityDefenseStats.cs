using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Defense Stats", menuName = "Engine/Entities/New Entity Defense Stats")]
public class EntityDefenseStats : ScriptableObject {

    [SerializeField]
    int health;
    [SerializeField]
    int armor;

    public int Health { get { return health; } }
    public int Armor { get { return armor; } }

}
