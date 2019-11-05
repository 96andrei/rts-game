using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDestroyEvent : MonoBehaviour {

    [SerializeField]
    Entity entity;
    [SerializeField]
    GameObject replaceWith;

    private void OnEnable()
    {
        entity.OnEntityDeath += Entity_OnEntityDeath;
    }

    private void Entity_OnEntityDeath()
    {
        var replacement = Instantiate(replaceWith);
        replacement.transform.position = entity.transform.position;
        replacement.transform.eulerAngles = entity.transform.eulerAngles;

        entity.OnDeath();
    }

    private void OnDisable()
    {
        entity.OnEntityDeath -= Entity_OnEntityDeath;
    }
}
