using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackControl : MonoBehaviour {

    protected EntityAttack attacker;
    protected Entity target;

    public event System.Action<Entity> OnAttackStart;

    public void SetAttacker(EntityAttack attacker)
    {
        this.attacker = attacker;
    }

    /// <summary>
    /// Called by the Animator
    /// </summary>
    protected abstract void OnAttack();

    public virtual void Fire(Entity target)
    {
        this.target = target;

        if (OnAttackStart != null)
            OnAttackStart(target);
    }

    public abstract bool Aimed(Entity target);

    public abstract bool Aimed(Entity target, Vector3 position);

}
