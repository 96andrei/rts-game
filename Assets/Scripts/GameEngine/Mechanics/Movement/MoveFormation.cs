using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MoveFormation
{

    protected Vector3[] unitPositions;
    protected List<Entity> selection;
    protected VirtualLeader leader;
    public VirtualLeader Leader { get { return leader; } }
    private float baseLeaderOffset = 1;
    protected float leaderOffset = 0;
    public float LeaderOffset { get { return leaderOffset; } }

    private Vector3 lastLeaderPosition;
    protected float unitDrift = 1;
    public float UnitDrift { get { return unitDrift; } }

    HashSet<Entity> unitsInPosition = new HashSet<Entity>();
    protected Vector3 destination;
    public Vector3 Destination { get { return destination; } }

    public int UnitCount { get { return selection.Count; } }

    protected Transform transformDestination;

    public MoveFormation(List<Entity> selection, Transform destination) : this(selection, destination.position)
    {
        transformDestination = destination;
    }

    public MoveFormation(List<Entity> selection, Vector3 destination)
    {
        this.selection = selection;

        leader = NavigationLeaderPool.GetLeader();
        leader.Formation = this;
        NavMeshQueryFilter filter = new NavMeshQueryFilter();
        filter.agentTypeID = leader.NavMeshAgent.agentTypeID;
        filter.areaMask = NavMesh.AllAreas;

        Vector3 position = GetMeanPosition();
        int searchThreshold = 50;
        int searchRange = 10;
        int offset = 1;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, searchRange, NavMesh.AllAreas))
            destination = hit.position;
        else
        {
            //TODO verifica daca la doilea parametru e destination sau position
            while (!NavMesh.Raycast(destination.Add2(Random.insideUnitCircle.normalized * offset), destination, out hit, NavMesh.AllAreas) && offset < searchThreshold)
                offset++;

            if (offset >= searchThreshold)
                Debug.LogError("1");
            destination = hit.position;
        }

        if (NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas))
        {
            /*Vector3 ret = hit.position;
            Vector3 pathDir = position - ret;
            ret += pathDir.normalized * (leader.NavMeshAgent.radius / 2);*/
            position = hit.position;
        }
        else
        {
            offset = 0;
            while (!NavMesh.Raycast(position.Add2(Random.insideUnitCircle.normalized * (selection.Count - 1 + offset)), position, out hit, NavMesh.AllAreas) && offset < searchThreshold)
                offset++;

            if (offset >= searchThreshold)
                Debug.LogError("2");
        }

        //dir.y = 0;
        //dir.Normalize();
        //position += dir * LeaderOffset;
        //leader.transform.position = position;
        leader.gameObject.SetActive(true);
        leader.NavMeshAgent.Warp(hit.position);

        if (!leader.NavMeshAgent.SetDestination(destination))
            Debug.LogError("3");

        //leader.NavMeshAgent.SamplePathPosition(1, 5, out hit);
        //leader.transform.LookAt(hit.position);

        this.destination = leader.NavMeshAgent.destination;

        PredictUnitPositions();
        AssignUnitPositions();

        for (int i = 0; i < unitPositions.Length; i++)
            if (-unitPositions[i].z > leaderOffset)
                leaderOffset = -unitPositions[i].z;
        leaderOffset /= 2;
        leaderOffset += baseLeaderOffset;

        EventManager.Instance.AddListener<EntityDeathEvent>(OnEntityDeath);
    }

    public void ChangeDestination(Transform destination)
    {
        transformDestination = destination;
        ChangeDestination(destination.position);
    }

    public void ChangeDestination(Vector3 destination)
    {
        this.destination = destination;
        if (leader == null)
        {
            leader = NavigationLeaderPool.GetLeader();
            PrepareLeader();
        }
        leader.gameObject.SetActive(true);
        leader.NavMeshAgent.SetDestination(destination);
    }

    private void PrepareLeader()
    {
        NavMeshHit hit;
        Vector3 position = GetMeanPosition();
        if (NavMesh.SamplePosition(position, out hit, 10, 1))
            position = hit.position;
        leader.NavMeshAgent.Warp(position);
        leader.Formation = this;
    }

    private void OnEntityDeath(EntityDeathEvent entityDeath)
    {
        RemoveEntity(entityDeath.Entity);
    }

    public void RemoveEntity(Entity entity)
    {
        selection.Remove(entity);
        unitsInPosition.Remove(entity);

        if (selection.Count == 0)
            return;

        if (leader == null || !leader.gameObject.activeInHierarchy)
            return;

        PredictUnitPositions();
        AssignUnitPositions();

        leader.SetSpeed(GetMinSpeed());
    }

    public abstract Vector3 Size();

    public void Update()
    {
        if (transformDestination != null && transformDestination.position != destination)
        {
            destination = transformDestination.gameObject.GetCachedEntity().UnitColider.ClosestPointOnBounds(leader.transform.position);
            leader.NavMeshAgent.SetDestination(destination);
        }
    }

    public float GetMinSpeed()
    {
        float minSpeed = selection[0].GetCachedComponent<IMovable>().Speed;
        for (int i = 1; i < selection.Count; i++)
        {
            float speed = selection[i].GetCachedComponent<IMovable>().Speed;
            if (speed < minSpeed)
                minSpeed = speed;
        }
        return minSpeed;
    }

    public void UpdateLeader(VirtualLeader leader)
    {
        this.leader = leader;
        if (leader == null)
            return;

        lastLeaderPosition = leader.transform.position;

    }

    public Vector3 GetLeaderPosition()
    {
        return lastLeaderPosition;
    }

    public void SetInPosition(Entity e, bool inPosition)
    {
        if (inPosition)
            unitsInPosition.Add(e);
        else
            unitsInPosition.Remove(e);
    }

    public bool Formed {
        get { return unitsInPosition.Count == selection.Count; }
    }

    public bool Active {
        get { return leader != null && leader.gameObject.activeInHierarchy && selection.Count > 0; /* && InactiveUnitCount < selection.Count;*/ }
    }

    public abstract void AssignUnitPositions();
    public abstract void PredictUnitPositions();

    public Vector3 GetUnitPosition(Entity e)
    {
        int index = selection.FindIndex((x) => x.Equals(e));
        if (index < 0)
            return Vector3.zero;

        return unitPositions[index];
    }

    public void OrderByTarget(Vector3 target)
    {
        selection.Sort((x, y) =>
        {
            return Vector3.Distance(x.transform.position, target).CompareTo(Vector3.Distance(y.transform.position, target));
        });
    }

    public int GetUnitIndex(Entity e)
    {
        return selection.IndexOf(e);
    }

    public Entity GetEntity(int index)
    {
        return selection[index];
    }

    public Vector3 GetExpectedPosition()
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < unitPositions.Length; i++)
            result += unitPositions[i];
        result /= unitPositions.Length;

        return Leader.transform.TransformPoint(result);
    }

    public virtual Vector3 GetMeanPosition()
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < selection.Count; i++)
            result += selection[i].transform.position;
        result /= selection.Count;
        //result.y = 0;
        return result;

    }

    public override abstract int GetHashCode();
}
