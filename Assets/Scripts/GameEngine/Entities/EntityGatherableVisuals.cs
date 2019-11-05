using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityGatherable))]
public class EntityGatherableVisuals : MonoBehaviour {

    [SerializeField]
    private Entity entity;
    private EntityGatherable gatherable;

    [SerializeField]
    private GameObject[] toDisable;
    
	void Awake () {
        gatherable = entity.GetCachedComponent<EntityGatherable>();

        gatherable.OnUnitTaken += Gatherable_OnUnitTaken;
	}

    private void Gatherable_OnUnitTaken()
    {
        if (!gatherable.Empty())
            return;

        foreach (var g in toDisable)
            g.SetActive(false);
    }

    private void OnDestroy()
    {
        gatherable.OnUnitTaken -= Gatherable_OnUnitTaken;
    }

}
