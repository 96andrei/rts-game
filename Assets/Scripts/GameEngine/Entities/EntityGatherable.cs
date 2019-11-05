using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityGatherable : MonoBehaviour, ITargetable
{
    [SerializeField]
    EntityGatherableStats stats;
    public EntityGatherableStats Stats { get { return stats; } }

    int units;
    public int Units { get { return units; } }

    Entity entity;

    public event System.Action OnUnitTaken;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        units = stats.Units;
    }

    public ICommand Resolve(List<Entity> entities, Vector3 hitPosition)
    {
        bool found = false;
        foreach (var e in entities)
            if (e.GetCachedComponent<IGatherer>() != null)
            {
                found = true;
                break;
            }

        if (found && !Empty())
            return new GatherCommand(entities, entity);

        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(hitPosition, out hit, 100, 1);

        return new MoveCommand(entities, hit.position);
    }

    public bool Empty()
    {
        return units == 0;
    }

    public int TakeUnit()
    {
        if (units == 0)
            return 0;

        units--;
        if (OnUnitTaken != null)
            OnUnitTaken();
        return stats.ValuePerUnit;
    }
}
