using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackStateFactory {

    public enum Type
    {
        Passive = 0,
        Defensive = 1,
        Aggresive = 2,
        Targeted = 3
    }

    public static AttackState CreateState(EntityAttack entityAttack)
    {
        return new PassiveAttackState(entityAttack);
    }

    public static AttackState CreateState(EntityAttack entityAttack, Vector3 pivot, Type state)
    {
        switch (state)
        {
            case Type.Passive:
                return new PassiveAttackState(entityAttack);
            case Type.Defensive:
                return new DefensiveAttackState(entityAttack, pivot);
            case Type.Aggresive:
                return new AggresiveAttackState(entityAttack, pivot);
            case Type.Targeted:
                return new TargetedAttackState(entityAttack);
        }

        throw new System.Exception("Invalid factory state parameter: " + state.ToString());
        
    }



}
