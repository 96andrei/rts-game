using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {

    void TakeDamage(AttackValue value, EntityAttack atatcker, float multiplier = 0);

}
