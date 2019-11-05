using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityGatherer : MonoBehaviour, IGatherer
{

    [SerializeField]
    EntityGathererStats stats;
    EntityGathererStats Stats { get { return stats; } }

    Entity entity;
    Entity target;
    public Entity Target { get { return target; } }
    IMovable movable;
    EntityGatherable gatherable;

    float progress = 0f;

    private void Awake()
    {
        entity = gameObject.GetCachedEntity();
        movable = entity.GetCachedComponent<IMovable>();
        entity.OnNewCommand += Entity_OnNewCommand;
    }

    private void Entity_OnNewCommand()
    {
        target = null;
        progress = 0;
    }

    public void Gather(Entity target, MoveFormation formation = null)
    {
        this.target = target;
        gatherable = target.GetCachedComponent<EntityGatherable>();

        //UnityEngine.AI.NavMeshHit hit;
        //UnityEngine.AI.NavMesh.SamplePosition(target.UnitColider.ClosestPointOnBounds(transform.position), out hit, 3, 1);

        movable.Move(formation.Destination, formation);
    }

    void Update()
    {

        if (!Gathering())
            return;

        movable.Stop();
        progress += Time.deltaTime * stats.GatherSpeed;

        if (progress >= gatherable.Stats.UnitGatherTime)
        {
            progress = 0;
            entity.Team.AddGold(gatherable.TakeUnit());
        }

    }

    public bool MovingTo()
    {
        return target != null;
    }

    public bool Gathering()
    {
        if (target == null)
            return false;

        if (gatherable.Empty())
        {
            target = null;
            return false;
        }

        Vector3 targetPosition = target.UnitColider.ClosestPointOnBounds(transform.position);

        var dist = Vector3.Distance(transform.position, targetPosition);
        var gatherDistance = stats.GatherRange + entity.GetUnitSize().x;
        Vector3 targetToBoundsDir = (targetPosition - target.transform.position).normalized;

        //if close enough to the mine, try to find an empty mining spot
        if (dist <= 10 && dist > gatherDistance || !movable.WorkingFormation() && dist > gatherDistance)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.transform.position + targetToBoundsDir * 0.1f, out hit, 20, NavMesh.AllAreas))
                movable.Move(hit.position, null);
        }

        if (dist > gatherDistance)
            return false;

        return true;
    }

    private void OnDestroy()
    {
        entity.OnNewCommand -= Entity_OnNewCommand;
    }
}
