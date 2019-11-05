using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker  {

    void ChangeState(AttackStateFactory.Type state);
    void Attack(Entity target, MoveFormation formation);
    void SetTarget(Entity target);
}
