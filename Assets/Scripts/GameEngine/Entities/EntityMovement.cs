
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EntityMovement : MonoBehaviour, IMovable {

    [SerializeField]
    Entity entity;

    [SerializeField]
    NavMeshAgent navMeshAgent;

    [SerializeField]
    NavMeshObstacle navMeshObstacle;

    bool working = false;

    MoveFormation formation;
    public MoveFormation Formation { get { return formation; } }

    float startSpeed;
    public float Speed { get { return startSpeed; } }

    public NavMeshAgent Agent { get { return navMeshAgent; } }

    public bool Suspended { get; set; }

    private void Awake()
    {
        navMeshAgent.avoidancePriority = Random.Range(0, 99);
        startSpeed = navMeshAgent.speed;
        entity.OnEntityDeath += Entity_OnDeath;
    }

    private void Entity_OnDeath()
    {
        if(navMeshAgent.enabled)
            navMeshAgent.isStopped = true;
        enabled = false;
    }

    public void Move(Vector3 destination, MoveFormation formation)
    {

        ChangeFormation(formation);

        if (working)
            return;

        navMeshAgent.speed = startSpeed;

        if (formation == null)
            StartCoroutine(MoveRoutine(destination));
        else
        {
            navMeshAgent.avoidancePriority = formation.GetUnitIndex(entity);
            StartCoroutine(MoveRoutine(formation.Leader.GetUnitTargetPosition(entity)));
        }

    }

    IEnumerator MoveRoutine(Vector3 destination)
    {
        if (working || Suspended)
            yield break;

        working = true;
        if (navMeshObstacle.enabled)
        {
            navMeshObstacle.enabled = false;
            yield return null;
        }
        navMeshAgent.updateRotation = true;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(destination);

        working = false;
    }

    public void Update()
    {

        if (working)
            return;

        if (WorkingFormation())
        {
            Vector3 targetPosition = formation.Leader.GetUnitTargetPosition(entity);
            Debug.DrawLine(transform.position, targetPosition);
            StartCoroutine(MoveRoutine(targetPosition));
            UpdateSpeed(targetPosition);
        }

        bool formationCondition = false;
        if (formation == null)
            formationCondition = true;
        else
            formationCondition = !formation.Active;

        if (navMeshAgent.enabled && !navMeshAgent.pathPending && formationCondition)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Stop();
                }
            }
        }

    }

    private void ChangeFormation(MoveFormation formation)
    {

        if (this.formation == formation)
            return;

        if(this.formation != null)
            this.formation.RemoveEntity(entity);
        this.formation = formation;
    }


    private void UpdateSpeed(Vector3 targetPosition)
    {
        if (formation == null)
            return;

        float drift = formation.UnitDrift;
        float speedModifier = 1;

        float distZ = transform.InverseTransformPoint(targetPosition).z;
        float distX = transform.InverseTransformPoint(targetPosition).x;

        float dist = (distZ + distX) / 2f;

        if (dist > 0 && dist > drift)
            speedModifier = dist / drift;
        else if (dist < 0 && Mathf.Abs(dist) > drift)
            speedModifier = Mathf.Abs(dist) / drift;

        speedModifier = Mathf.Clamp(speedModifier, 0, 1f);

        navMeshAgent.speed = startSpeed * speedModifier;

        formation.SetInPosition(entity, speedModifier == 1);

    }

    public void Stop()
    {
        if (!navMeshAgent.enabled || working)
            return;

        navMeshAgent.ResetPath();
        StartCoroutine(StopRoutine());
    }

    IEnumerator StopRoutine()
    {
        working = true;
        if (Formation != null)
        {
            Formation.RemoveEntity(entity);
            ChangeFormation(null);
        }

        if (navMeshAgent.enabled)
        {
            navMeshAgent.enabled = false;
            yield return null;
        }
        navMeshObstacle.enabled = true;

        working = false;
    }

    public bool WorkingFormation()
    {
        return formation != null && formation.Active;
    }
}
