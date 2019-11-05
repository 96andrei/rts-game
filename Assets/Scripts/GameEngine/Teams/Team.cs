using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{

    public string Name { get; private set; }
    public TeamList TeamList { get; private set; }
    public int Id { get; private set; }
    public Color Color { get; private set; }
    public bool HumanControlled { get; private set; }

    //TODO USE CONSTANTS FILE
    int gold = 50;
    public int Gold { get { return gold; } }

    public event System.Action OnTeamStateChanged;

    private List<Entity> units;

    FormationFactory formationFactory = new FormationFactory();
    public FormationFactory FormationFactory { get { return formationFactory; } }

    public Team(int id, string name, TeamList teamList, Color color, bool humanControlled)
    {
        this.Id = id;
        this.Name = name;
        this.TeamList = teamList;
        this.Color = color;
        this.HumanControlled = humanControlled;
        units = new List<Entity>();
        EventManager.Instance.AddListener<EntityDeathEvent>(OnEntityDeath);
    }

    public void AddUnit(Entity entity)
    {
        units.Add(entity);
        entity.Team = this;

        if (OnTeamStateChanged != null)
            OnTeamStateChanged();
    }

    public int UnitCount()
    {
        return units.Count;
    }

    public void ForEach(Action<Entity> action)
    {
        for (int i = 0; i < units.Count; i++)
            action(units[i]);
    }

    public void OnEntityDeath(EntityDeathEvent deathEvent)
    {
        if (deathEvent.Entity.Team != this)
            return;

        if (units.Contains(deathEvent.Entity))
            units.Remove(deathEvent.Entity);

        if (OnTeamStateChanged != null)
            OnTeamStateChanged();
    }

    public void AddGold(int value)
    {
        if (value <= 0)
            return;

        gold += value;

        if (OnTeamStateChanged != null)
            OnTeamStateChanged();
    }

    public void UseGold(int value)
    {
        if (value <= 0)
            return;

        gold -= value;

        if (OnTeamStateChanged != null)
            OnTeamStateChanged();
    }

    public bool CanAfford(int value)
    {
        return value <= gold;
    }
}
