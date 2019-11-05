using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackControl : AttackControl
{

    static Dictionary<GameObject, List<GameObject>> projectileCache = new Dictionary<GameObject, List<GameObject>>();

    [SerializeField]
    GameObject projectilePrefab;

    [SerializeField]
    AnimationCurve trajectory;

    [SerializeField]
    AnimationCurve offsetCurve;

    [SerializeField]
    Transform spawnPoint;
    public Transform SpawnPoint { get { return spawnPoint; } }

    Dictionary<GameObject, Entity> projectileTargets = new Dictionary<GameObject, Entity>();
    Dictionary<GameObject, Vector3> projectileTargetPosition = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> projectileTimes = new Dictionary<GameObject, float>();
    Dictionary<GameObject, Vector3> projectileStarting = new Dictionary<GameObject, Vector3>();
    Dictionary<GameObject, float> projectileDistance = new Dictionary<GameObject, float>();
    Dictionary<GameObject, float> projectileVerticalDistanceMultiplier = new Dictionary<GameObject, float>();

    List<GameObject> toRemove = new List<GameObject>();

    Vector3 lastTargetPosition;

    /// <summary>
    /// Called by the Animator
    /// </summary>
    protected override void OnAttack()
    {
        if (!target.Available())
            return;

        if (spawnPoint == null)
        {
            attacker.DealDamage(target.GetComponent<IDamageable>());
            return;
        }
        
        GameObject p = GetProjectileFromPool();//Instantiate(projectilePrefab);
        p.gameObject.SetActive(false);
        p.transform.position = spawnPoint.position;
        projectileDistance[p] = Vector3.Distance(spawnPoint.position, target.transform.position);
        projectileTargets[p] = target;
        projectileTargetPosition[p] = target.transform.position + Vector3.up * 0.5f;
        projectileStarting[p] = spawnPoint.position;
        projectileTimes[p] = 0;
        projectileVerticalDistanceMultiplier[p] = 0;
        p.gameObject.SetActive(true);
    }

    public void Update()
    {
        for (int i = 0; i < toRemove.Count; i++)
            RemoveProjectile(toRemove[i]);
        toRemove.Clear();

        float inverseSpeed = 1 / attacker.Stats.ProjectileSpeed;

        foreach (GameObject p in projectileTargets.Keys)
        {
            Entity target = projectileTargets[p];
            /*if (!target.Available())
            {
                toRemove.Add(p);
                continue;
            }*/
            float time = projectileTimes[p];
            float duration = projectileDistance[p] * inverseSpeed;
            Vector3 start = projectileStarting[p];

            Vector3 baseTarget = target.Available() ? target.transform.position : projectileTargetPosition[p];

            if (target.Available())
                projectileTargetPosition[p] = target.transform.position;

            Vector3 targetPosition = baseTarget + new Vector3(0, spawnPoint.localPosition.y, 0);
            float yOffset = offsetCurve.Evaluate(projectileDistance[p]);

            p.transform.LookAt(targetPosition);
            var position = Vector3.Lerp(start, targetPosition, time / duration);
            position.y += trajectory.Evaluate(time / duration) * yOffset;
            p.transform.LookAt(position);

            projectileVerticalDistanceMultiplier[p] += position.y - p.transform.position.y;

            p.transform.position = position;



            time += Time.deltaTime;

            projectileTimes[p] = time;

            if (time >= duration)
            {
                toRemove.Add(p);
                float multiplier = -projectileVerticalDistanceMultiplier[p] * attacker.Stats.ProjectileVerticalDamageModifier;
                multiplier = Mathf.Clamp(multiplier, -0.5f, 2);
                attacker.DealDamage(target.GetCachedComponent<IDamageable>(), multiplier);
            }
        }

    }

    void RemoveProjectile(GameObject p)
    {
        projectileTargets.Remove(p);
        projectileDistance.Remove(p);
        projectileStarting.Remove(p);
        projectileTimes.Remove(p);
        projectileTargetPosition.Remove(p);
        p.SetActive(false);
        SendProjectileToPool(p);
    }

    public override bool Aimed(Entity toAttack)
    {
        return Aimed(toAttack, spawnPoint.position);
    }

    public override bool Aimed(Entity toAttack, Vector3 position)
    {
        if (toAttack == null)
            return false;

        Vector3 targetPosition = toAttack.UnitColider.ClosestPointOnBounds(attacker.transform.position);

        if (Vector3.Distance(position, targetPosition) > attacker.Stats.Range)
            return false;

        var spawnHorizPosition = spawnPoint.forward;
        spawnHorizPosition.y = 0;

        var targetHorizPosition = targetPosition - transform.position;
        targetHorizPosition.y = 0;

        var horizAngle = Vector3.Angle(spawnHorizPosition, targetHorizPosition);

        var spawnVertPosition = spawnPoint.forward;
        spawnVertPosition.x = 0;
        spawnVertPosition.z = 0;

        var targetVertPosition = targetPosition - transform.position;
        targetVertPosition.x = 0;
        targetVertPosition.z = 0;

        var vertAngle = Vector3.Angle(spawnVertPosition, targetVertPosition);

        return horizAngle <= attacker.Stats.HorizontalAngle && vertAngle <= attacker.Stats.VerticalAngle;
    }

    public GameObject GetProjectileFromPool()
    {
        GameObject result = null;
        List<GameObject> target;
        if (projectileCache.TryGetValue(projectilePrefab, out target))
        {
            if (target.Count == 0)
                result = Instantiate(projectilePrefab);
            else
            {
                result = target[target.Count - 1];
                target.Remove(result);
            }

        }
        else
        {
            target = new List<GameObject>();
            projectileCache[projectilePrefab] = target;

            result = Instantiate(projectilePrefab);
        }
        return result;
    }

    public void SendProjectileToPool(GameObject projectile)
    {
        projectileCache[projectilePrefab].Add(projectile);
    }
}
