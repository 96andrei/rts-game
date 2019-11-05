using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent {

	
}

public class SelectionEvent : GameEvent
{

    public List<Entity> Selection { get; private set; }
    public bool StaticObject { get; private set; }

    public SelectionEvent(List<Entity> selection)
    {
        this.Selection = selection;

        if (selection != null && selection.Count > 0 && selection[0].GetCachedComponent<IMovable>() == null)
            StaticObject = true;
    }
}

public class StartSelectBoxEvent : GameEvent
{

    public Vector3 Anchor { get; private set; }

    public StartSelectBoxEvent(Vector3 anchor)
    {
        this.Anchor = anchor;
    }
}

public class DragSelectBoxEvent : GameEvent
{

    public Vector3 Outer { get; private set; }

    public DragSelectBoxEvent(Vector3 outer)
    {
        this.Outer = outer;
    }
}

public class MoveEvent : GameEvent
{
    public Vector3 Point { get; private set; }
    public List<Entity> Entities { get; private set; }

    public MoveEvent(Vector3 point, List<Entity> entities)
    {
        Point = point;
        Entities = entities;
    }

}

public class AttackEvent : GameEvent
{
    public Entity Target { get; private set; }
    public List<Entity> Entities { get; private set; }

    public AttackEvent(Entity target, List<Entity> entities)
    {
        Target = target;
        Entities = entities;
    }
}

public class EntityDeathEvent : GameEvent
{
    public Entity Entity { get; private set; }

    public EntityDeathEvent(Entity entity)
    {
        Entity = entity;
    }
}

public class GatherEvent : GameEvent
{
    public Entity Target { get; private set; }
    public List<Entity> Entities { get; private set; }

    public GatherEvent(Entity target, List<Entity> entities)
    {
        Target = target;
        Entities = entities;
    }
}