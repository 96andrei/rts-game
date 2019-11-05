using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour, IAttacker
{
    [SerializeField]
    Entity entity;
    public Entity Entity { get { return entity; } }
    IMovable movable;
    public IMovable Movable { get { return movable; } }

    [SerializeField]
    AttackControl attackControl;
    public AttackControl AttackControl { get { return attackControl; } }

    [SerializeField]
    EntityAttackStats stats;

    [SerializeField]
    SphereCollider attackArea;

    public EntityAttackStats Stats { get { return stats; } }

    private float cooldown;

    Entity target;
    public Entity Target { get { return target; } }
    IDamageable targetDamageable;

    bool attackingState = false;
    bool foundTarget = false;
    public bool FoundTarget { get { return foundTarget; } }

    List<Entity> inRange = new List<Entity>();
    List<Entity> toRemoveFromRange = new List<Entity>();

    //cached items
    RaycastHit hit;

    AttackState attackState;

    private void Awake()
    {
        movable = GetComponent<IMovable>();
        attackArea.radius = stats.SearchRange;
        attackControl.SetAttacker(this);
        attackState = new DefensiveAttackState(this, transform.position);
    }

    private void Start()
    {
        entity.OnNewCommand += OnNewCommand;
        EventManager.Instance.AddListener<MoveEvent>((command) =>
        {
            if (command.Entities.Contains(entity))
            {
                Vector3 target = command.Point;
                if (movable.WorkingFormation())
                {
                    target = movable.Formation.Destination + movable.Formation.GetUnitPosition(entity);
                }

                attackState = new DefensiveAttackState(this, target);
            }
        });

        EventManager.Instance.AddListener<GatherEvent>((command) =>
        {
            if (command.Entities.Contains(entity))
            {
                attackState = new PassiveAttackState(this);
            }
        });
    }

    private void OnDestroy()
    {
        entity.OnNewCommand -= OnNewCommand;
    }

    private void OnNewCommand()
    {
        attackingState = false;
        target = null;
        targetDamageable = null;
    }

    public void Attack(Entity target, MoveFormation formation)
    {
        attackState = new TargetedAttackState(this);
        Movable.Move(target.UnitColider.ClosestPointOnBounds(transform.position), formation);
        SetTarget(target);
    }

    public void SetTarget(Entity target)
    {
        this.target = target;

        if (target == null)
            return;

        targetDamageable = target.GetCachedComponent<IDamageable>();
    }

    public void DealDamage(IDamageable damageable, float multiplier = 0)
    {
        damageable.TakeDamage(stats.Attack, this, multiplier);
    }


    public void Fire(Entity target)
    {
        if (cooldown > 0)
            return;
        attackControl.Fire(target);
        cooldown = stats.Rate;
    }

    private void Update()
    {

        if (!entity.Available())
            return;

        UpdateRange();

        if (cooldown > 0)
            cooldown -= Time.deltaTime;

        attackState.UpdateState();

    }

    public void RotateTowards(Entity target)
    {
        var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), movable.Agent.angularSpeed * Time.deltaTime);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = rot;

    }

    public void ToDefaultAttackState()
    {
        attackState = new DefensiveAttackState(this, transform.position);
    }

    private void UpdateRange()
    {
        toRemoveFromRange.Clear();
        foreach (var ent in inRange)
        {
            if (ent == null)
            {
                toRemoveFromRange.Add(ent);
                continue;
            }
            if (entity.IsOpponent(ent))
            {
                if (!Target.Available())
                    SetTarget(ent);
                attackState.OnRangeEntered(ent);
            }
        }
        foreach (var ent in toRemoveFromRange)
            RemoveFromRange(ent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        Entity oEntity = other.gameObject.GetCachedEntity();
        if (oEntity == null)
            return;

        inRange.Add(oEntity);
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.isTrigger)
            return;

        Entity oEntity = other.gameObject.GetCachedEntity();
        if (oEntity == null)
            return;

        RemoveFromRange(oEntity);
        if (oEntity == target)
            target = null;

        if (!target.Available())
            return;

        if (other.gameObject.Equals(target.gameObject))
            attackState.OnRangeExit(oEntity);

    }

    private void RemoveFromRange(Entity e)
    {
        if (target == e)
            SetTarget(null);
        inRange.Remove(e);
    }

    public void ChangeState(AttackStateFactory.Type state)
    {
        attackState = AttackStateFactory.CreateState(this, movable.Agent.destination, state);
    }
}
