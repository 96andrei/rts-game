using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ignores enemies around
/// </summary>
public class PassiveAttackState : AttackState
{
    public PassiveAttackState(EntityAttack entityAttack) : base(entityAttack)
    {

    }

    public override void OnRangeEntered(Entity other)
    {
    }

    public override void OnRangeExit(Entity other)
    {
    }

    public override void UpdateState()
    {
        
    }
}
