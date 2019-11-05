using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceEntity : InfluenceObject {

    Entity entity;

    private void Awake()
    {
        entity = gameObject.GetCachedEntity();
    }

    private void Start()
    {
        if (entity.Team.Id == 1)
            strength = -strength;
    }

}
