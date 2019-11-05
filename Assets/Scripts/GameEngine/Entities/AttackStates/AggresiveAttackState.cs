using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attacks while holding position
/// </summary>
public class AggresiveAttackState : AttackState
{
    Vector3 pivot;

    private HashSet<Entity> inRange = new HashSet<Entity>();
    private List<Entity> toRemove = new List<Entity>();

    public AggresiveAttackState(EntityAttack entityAttack, Vector3 pivot) : base(entityAttack)
    {
        this.pivot = pivot;
    }

    public override void OnRangeEntered(Entity other)
    {

        inRange.Add(other);

        float maxDist = 100000;
        Entity target = null;
        foreach(var ent in inRange) {
            if (!ent.Available())
            {
                toRemove.Add(ent);
                continue;
            }

            if (ent.GetCachedComponent<EntityMetadata>().Type.TypeId.Equals("e_building") && inRange.Count > 1)
                continue;

            float dist = Vector3.Distance(ent.transform.position, entityAttack.transform.position);
            if (dist < maxDist)
            {
                maxDist = dist;
                target = ent;
            }
        }

        foreach (var e in toRemove)
            inRange.Remove(e);
        toRemove.Clear();

        entityAttack.SetTarget(target);
    }

    public override void OnRangeExit(Entity other)
    {
        inRange.Remove(other);
    }

    public override void UpdateState()
    {
        UpdatePivot();

        if (!entityAttack.Target.Available())
        {
            ReturnToPivot();
            return;
        }

        Vector3 targetPosition = entityAttack.Target.UnitColider.ClosestPointOnBounds(entityAttack.transform.position);

        //float distancePivotToTarget = Vector3.Distance(entityAttack.Target.transform.position, pivot);

        float distanceToTarget = Vector3.Distance(entityAttack.transform.position, targetPosition);
        if (!entityAttack.AttackControl.Aimed(entityAttack.Target) && distanceToTarget > entityAttack.Stats.Range)
        {
            entityAttack.Movable.Move(targetPosition, null);
            return;
        }

        if (distanceToTarget > entityAttack.Stats.SearchRange)
        {
            ReturnToPivot();
            return;
        }

        entityAttack.Movable.Agent.updateRotation = false;
        entityAttack.Movable.Stop();
        entityAttack.RotateTowards(entityAttack.Target);

        if (entityAttack.AttackControl.Aimed(entityAttack.Target))
            entityAttack.Fire(entityAttack.Target);

    }

    void UpdatePivot()
    {
        if (entityAttack.Movable.WorkingFormation())
            pivot = entityAttack.Movable.Formation.Leader.GetUnitTargetPosition(entityAttack.Entity);
    }

    void ReturnToPivot()
    {
        if (Vector3.Distance(pivot, entityAttack.Movable.Agent.destination) < entityAttack.Movable.Agent.stoppingDistance * 2)
            return;

        if (Vector3.Distance(entityAttack.transform.position, pivot) > entityAttack.Movable.Agent.stoppingDistance * 2)
            entityAttack.Movable.Move(pivot, null);
    }
}
