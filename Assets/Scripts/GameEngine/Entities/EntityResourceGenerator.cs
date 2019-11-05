using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EntityResourceGenerator : MonoBehaviour {

    Entity entity;

    [SerializeField]
    EntityGeneratorStats stats;


    float cooldown;

    private void Awake()
    {
        entity = gameObject.GetCachedEntity();
        ResetCooldown();
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if(cooldown <= 0)
        {
            entity.Team.AddGold(stats.Value);
            ResetCooldown();
        }
        
    }

    private void ResetCooldown()
    {
        cooldown = stats.Interval;
    }

}
