using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VirtualLeader : MonoBehaviour {

    [SerializeField]
    NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    private MoveFormation formation;
    public MoveFormation Formation {
        get { return formation; }
        set {
            formation = value;
            speed = formation.GetMinSpeed();
        }
    }

    float speed = 10f;

    [SerializeField]
    float maxDrift = 2f;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        navMeshAgent.speed = speed;
    }

    private void Update()
    {

        if (Formation == null || !Formation.Active)
        {
            gameObject.SetActive(false);
            //Debug.Log("Formation not active");
            return;
        }

        StartCoroutine(DrawPath(NavMeshAgent.path));

        Formation.UpdateLeader(this);
        UpdateSpeed();

        if (!formation.Formed)
            return;

        if (navMeshAgent.enabled && !navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                /*if(navMeshAgent.path.status == NavMeshPathStatus.PathInvalid)
                {
                    if (Vector3.Distance(transform.position, formation.Destination) <= 1)
                    {
                        gameObject.SetActive(false);
                        return;
                    }

                    NavMeshHit hit;
                    NavMesh.SamplePosition(formation.Destination.Add2(Random.insideUnitCircle.normalized * 2), out hit, 20, NavMesh.AllAreas);
                    //Debug.Log("Path invalid. New dest:" + hit.position + " " + formation.GetEntity(0));
                    while (!NavMeshAgent.Warp(hit.position)) ;
                    Formation.ChangeDestination(NavMeshAgent.transform.position);
                    return;
                }*/

                if (!navMeshAgent.hasPath /*&& navMeshAgent.path.status == NavMeshPathStatus.PathComplete*/ || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    //if(!navMeshAgent.hasPath)
                        //Debug.Log(gameObject + " " + navMeshAgent.hasPath + " " + navMeshAgent.velocity.sqrMagnitude + " " + navMeshAgent.path.status + " " + formation.GetEntity(0));
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public Vector3 GetTargetPosition(Vector3 relativePosition)
    {
        float dist = navMeshAgent.speed;
        NavMeshHit hit;
        navMeshAgent.SamplePathPosition(1, dist, out hit);
        Vector3 targetPosition = transform.TransformPoint(transform.InverseTransformPoint(hit.position) + relativePosition);
        if(NavMesh.SamplePosition(targetPosition, out hit, 10, 1))
            return hit.position;

        NavMesh.SamplePosition(transform.position, out hit, 10, 1);
        return hit.position;
    }

    public Vector3 GetUnitTargetPosition(Entity unit)
    {
        float distancePerFrame = navMeshAgent.speed * Time.fixedDeltaTime;
        NavMeshHit hit;
        navMeshAgent.SamplePathPosition(NavMesh.AllAreas, distancePerFrame, out hit);
        Vector3 relativeUnitPosition = Formation.GetUnitPosition(unit);
        Vector3 targetPosition = transform.TransformPoint(transform.InverseTransformPoint(hit.position) + relativeUnitPosition);
        NavMesh.SamplePosition(targetPosition, out hit, 30, NavMesh.AllAreas);
        return hit.position;
    }

    void UpdateSpeed()
    {
        Vector3 formationCenter = Formation.GetMeanPosition();
        formationCenter.y = 0;
        Vector3 pos = transform.position;
        pos.y = 0;

        float speedModifier = 1;

        float dist = (formationCenter - pos).sqrMagnitude - Formation.LeaderOffset * Formation.LeaderOffset;

        speedModifier = (maxDrift * maxDrift - dist) / (maxDrift * maxDrift);

        //speedModifier = Mathf.Clamp(speedModifier, 0f, 2f);

        if (!Formation.Formed)
            speedModifier = 0.5f;
        
        navMeshAgent.speed = speed * speedModifier;
    }

    public void SendToPool() { 
        if(Formation != null)
            Formation.UpdateLeader(null);
        NavigationLeaderPool.AddLeader(this);
    }

    private void OnDisable()
    {
        SendToPool();
    }

    IEnumerator DrawPath(NavMeshPath path)
    {
        yield return new WaitForEndOfFrame();
        path = NavMeshAgent.path;
        if (path.corners.Length < 2)
            yield break;

        Color c = Color.white;
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                c = Color.white;
                break;
            case NavMeshPathStatus.PathInvalid:
                c = Color.red;
                break;
            case NavMeshPathStatus.PathPartial:
                c = Color.yellow;
                break;
        }

        Vector3 previousCorner = path.corners[0];

        int i = 1;
        while (i < path.corners.Length)
        {
            Vector3 currentCorner = path.corners[i];
            Debug.DrawLine(previousCorner, currentCorner, c);
            previousCorner = currentCorner;
            i++;
        }

    }

}
