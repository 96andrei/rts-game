using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    GameObject halo;
    public GameObject Halo { get { return halo; } }

    public event System.Action OnNewCommand, OnEntityDeath, OnSpawn;

    [SerializeField]
    bool batchSelection = true;
    public bool BatchSelection { get { return batchSelection; } }

    [SerializeField]
    Collider unitCollider;
    public Collider UnitColider { get { return unitCollider; } }

    [SerializeField]
    float visibilityRange = 20;
    public float VisibilityRange { get { return visibilityRange; } }

    public Team Team { get; set; }

    public Level Level { get { return Team.TeamList.Level; } }

    public TeamList TeamList { get { return Team.TeamList; } }

    private Dictionary<System.Type, object> cachedComponents = new Dictionary<System.Type, object>();

    private void Start()
    {
        if (OnSpawn != null)
            OnSpawn();
    }

    public T GetCachedComponent<T>()
    {
        object value = null;
        if (cachedComponents.TryGetValue(typeof(T), out value))
            return (T)value;

        value = GetComponentInChildren<T>();

        cachedComponents[typeof(T)] = value;

        return (T)value;
    }

    public bool IsOpponent(Entity entity)
    {
        return Team != entity.Team && entity.Team != null && entity.Team.Id != 0 && !TeamList.AreAllied(Team.Id, entity.Team.Id);
    }

    public bool IsTeammate(Entity entity)
    {
        return Team == entity.Team;
    }

    public void OverrideCommand()
    {
        if (OnNewCommand != null)
            OnNewCommand();
    }

    public Vector3 GetUnitSize()
    {
        BoxCollider boxCol = unitCollider as BoxCollider;
        if (boxCol != null)
            return boxCol.size;

        CapsuleCollider capsCol = unitCollider as CapsuleCollider;
        if (capsCol != null)
            return new Vector3(capsCol.radius * 2, capsCol.height, capsCol.radius * 2);

        SphereCollider spCol = unitCollider as SphereCollider;
        if (capsCol != null)
            return new Vector3(spCol.radius * 2, spCol.radius * 2, spCol.radius * 2);

        return Vector3.one;

    }

    public void TriggerDeath()
    {
        enabled = false;
        if (OnEntityDeath != null)
            OnEntityDeath();

    }

    public void OnDeath()
    {

        Destroy(gameObject);
    }

}

