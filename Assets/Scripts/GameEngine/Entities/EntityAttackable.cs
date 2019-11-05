using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackable : MonoBehaviour, ITargetable, IDamageable
{
    Entity entity;

    [SerializeField]
    EntityDefenseStats stats;

    int health;
    int armor;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        health = stats.Health;
        armor = stats.Armor;
    }

    public ICommand Resolve(List<Entity> e, Vector3 hitPosition)
    {
        List<Entity> affected = new List<Entity>();

        foreach(Entity ent in e)
        {
            if (!ent.IsTeammate(entity))
                affected.Add(ent);
        }

        if (affected.Count == 0)
        {
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(hitPosition, out hit, 100, 1);
            return new MoveCommand(e, hit.position);
        }

        return new AttackCommand(affected, entity);
    }

    public void TakeDamage(AttackValue value, EntityAttack attacker, float multiplier)
    {
        if (health <= 0)
            return;

        //Debug.LogWarning("STANDARDS: CONSTANT VALUE");
        int remainingArmor = Mathf.Clamp(armor - value.ArmorPiercing, 0, 100);
        int damage = Mathf.Clamp(value.PhysicalDamage + (int)(value.PhysicalDamage * multiplier) - remainingArmor, 0, 100);
        health -= damage;

        if (health <= 0)
            entity.TriggerDeath();
        
    }

}
