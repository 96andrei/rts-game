using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackValue {

    [SerializeField]
    private int physicalDamage;
    [SerializeField]
    private int armorPiercing;

	public int PhysicalDamage { get { return physicalDamage; } }
    public int ArmorPiercing { get { return armorPiercing; } }

    public AttackValue(int physical, int pierce)
    {
        physicalDamage = physical;
        armorPiercing = pierce;
    }

}
