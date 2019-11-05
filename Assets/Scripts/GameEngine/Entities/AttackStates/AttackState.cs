using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState {

    protected EntityAttack entityAttack;

    public AttackState(EntityAttack entityAttack)
    {
        this.entityAttack = entityAttack;
    }

    public abstract void UpdateState();
    public abstract void OnRangeEntered(Entity other);
    public abstract void OnRangeExit(Entity other);
}
