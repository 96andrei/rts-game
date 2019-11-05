using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand  {
    void Execute();
}

public class MoveCommand : ICommand
{
    List<Entity> entities;
    Vector3 point;

    public MoveCommand(List<Entity> entities, Vector3 point)
    {
        this.entities = entities;
        this.point = point;
    }

    public void Execute()
    {
        int count = 0;

        var formation = entities[0].Team.FormationFactory.Get(entities, point);

        foreach (Entity e in entities)
        {
            IMovable movable = e.GetCachedComponent<IMovable>();
            if (movable == null)
                continue;
            count++;
            e.OverrideCommand();
            movable.Move(point, formation);
        }

        if (count == 0)
            return;
        EventManager.Instance.TriggerEvent(new MoveEvent(point, entities));
    }
}

public class AttackCommand : ICommand
{
    List<Entity> entities;
    Entity target;

    public AttackCommand(List<Entity> entities, Entity target)
    {
        this.entities = entities;
        this.target = target;
    }

    public void Execute()
    {
        var formation = entities[0].Team.FormationFactory.Get(entities, target.transform);

        foreach (Entity e in entities)
        {
            e.OverrideCommand();
            e.GetCachedComponent<IAttacker>().Attack(target, formation);
        }

        EventManager.Instance.TriggerEvent(new AttackEvent(target, entities));
       
    }
}

public class GatherCommand : ICommand
{
    List<Entity> entities;
    Entity target;

    public GatherCommand(List<Entity> entities, Entity target)
    {
        this.entities = entities;
        this.target = target;
    }

    public void Execute()
    {
        var targetPosition = target.UnitColider.ClosestPointOnBounds(entities[0].transform.position);
        var formation = entities[0].Team.FormationFactory.Get(entities, targetPosition);
        //UnityEngine.AI.NavMeshHit hit;
        //UnityEngine.AI.NavMesh.SamplePosition(target.transform.position, out hit, 10, 1);

        var gatherers = new List<Entity>();
        var defenders = new List<Entity>();
        
        foreach (Entity e in entities)
        {
            e.OverrideCommand();
            IGatherer gatherer = e.GetCachedComponent<IGatherer>();
            if (gatherer != null)
            {
                gatherers.Add(e);
                gatherer.Gather(target, formation);
            }
            else
            {
                e.GetCachedComponent<IMovable>().Move(targetPosition, formation);
                defenders.Add(e);
            }
        }

        EventManager.Instance.TriggerEvent(new MoveEvent(targetPosition, defenders));
        EventManager.Instance.TriggerEvent(new GatherEvent(target, gatherers));

    }
}