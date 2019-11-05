using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackControl : AttackControl
{
    public override bool Aimed(Entity target)
    {
        return Aimed(target, attacker.Entity.UnitColider.ClosestPointOnBounds(target.transform.position));
    }

    public override bool Aimed(Entity target, Vector3 position)
    {
        Vector3 targetPositon = target.UnitColider.ClosestPoint(transform.position);
        float dist = Vector3.Distance(position, targetPositon);
        return dist <= attacker.Stats.Range;
    }

    protected override void OnAttack()
    {
        attacker.DealDamage(target.GetCachedComponent<IDamageable>());
    }
}
