using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Attack Stats", menuName = "Engine/Entities/New Entity Attack Stats")]
public class EntityAttackStats : ScriptableObject {

    [SerializeField]
    AttackValue attack;

    [Tooltip("Used for cooldown")]
    [SerializeField]
    float rate;
    [SerializeField]
    float range = 2;
    [SerializeField]
    float horizontalAngle = 20;
    [SerializeField]
    float verticalAngle = 80;
    [SerializeField]
    float projectileSpeed = 1;
    [SerializeField]
    float projectileVerticalDamageModifier = 0.2f;
    [Tooltip("Animation speed - triggers OnAttack event")]
    [SerializeField]
    float attackSpeed = 1;
    [SerializeField]
    float holdingRange = 5;

    public AttackValue Attack { get { return attack; } }
    public float Rate { get { return rate; } }
    public float Range { get { return range; } }
    public float HorizontalAngle { get { return horizontalAngle; } }
    public float VerticalAngle { get { return verticalAngle; } }
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float HoldingRange { get { return holdingRange; } }
    public float SearchRange { get { return holdingRange + range; } }
    public float ProjectileVerticalDamageModifier { get { return projectileVerticalDamageModifier; } }

}
