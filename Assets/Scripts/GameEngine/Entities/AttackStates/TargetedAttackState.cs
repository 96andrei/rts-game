using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetedAttackState : AttackState {


    private bool stateFinished = false;
    private bool targetLost;

    public TargetedAttackState(EntityAttack entityAttack) : base(entityAttack)
    {

    }

    public void Dispose()
    {
        stateFinished = true;
        entityAttack.Movable.Move(entityAttack.transform.position, null);
        entityAttack.ToDefaultAttackState();
    }

    public override void OnRangeEntered(Entity other)
    {
        if (other == entityAttack.Target)
            targetLost = false;
    }

    public override void OnRangeExit(Entity other)
    {
        if(other == entityAttack.Target)
            targetLost = true;
    }

    public override void UpdateState()
    {
        if (stateFinished)
            return;

        if (targetLost || !entityAttack.Target.Available())
        {
            Dispose();
            return;
        }

        Vector3 targetPositon = entityAttack.Target.UnitColider.ClosestPointOnBounds(entityAttack.transform.position);

        float distanceToTarget = Vector3.Distance(entityAttack.transform.position, targetPositon);
        //float formationDistance = entityAttack.Movable.Formation != null && entityAttack.Movable.Formation.Leader != null ? Vector3.Distance(entityAttack.Movable.Formation.Leader.transform.position, targetPositon) : 0;
        if (!entityAttack.AttackControl.Aimed(entityAttack.Target) && distanceToTarget > entityAttack.Stats.Range)
        {
            //if(formationDistance <= entityAttack.Stats.Range)
               entityAttack.Movable.Move(targetPositon, null);
            return;
        }

        entityAttack.Movable.Agent.updateRotation = false;
        entityAttack.Movable.Stop();
        entityAttack.RotateTowards(entityAttack.Target);

        if (entityAttack.AttackControl.Aimed(entityAttack.Target))
            entityAttack.Fire(entityAttack.Target);

    }

}
