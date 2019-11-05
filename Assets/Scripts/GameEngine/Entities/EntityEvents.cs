using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Decouples Entity from EventManager
/// </summary>
[RequireComponent(typeof(Entity))]
public class EntityEvents : MonoBehaviour
{
    Entity entity;

    private void Start()
    {
        entity = GetComponent<Entity>();
        entity.OnEntityDeath += EntityOnDeath;
    }

    private void EntityOnDeath()
    {
        EventManager.Instance.TriggerEvent(new EntityDeathEvent(entity));
    }

}
