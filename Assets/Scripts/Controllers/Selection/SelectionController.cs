using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionController
{

    private Level level;
    private Team playerTeam;

    public List<Entity> Selection { get; private set; }

    private Vector3 anchor;
    private Vector3 outer;
    private bool hasActiveBox;

    public SelectionController(Level level)
    {
        this.level = level;
        Selection = new List<Entity>();
        this.playerTeam = level.PlayerTeam;
        EventManager.Instance.AddListener<EntityDeathEvent>(OnEntityDeathEvent);
    }

    public void ClearSelection()
    {
        RemoveAllSelections();

        EventManager.Instance.TriggerEvent(new SelectionEvent(Selection.ToList()));
    }

    private void AddToSelection(Entity entity)
    {
        //DebugUtil.Assert(entity != null, "Selected object with no entity");

        Selection.Add(entity);

        //entity.Transform.Find("Halo").gameObject.SetActive(true);
        entity.Halo.SetActive(true);
    }

    private void AddAllWithinBounds()
    {
        Bounds bounds = SelectUtils.GetViewportBounds(CameraUtils.Main, anchor, outer);

        this.level.PlayerTeam.ForEach((Entity entity) => {
            if (!entity.BatchSelection)
                return;

            if (SelectUtils.IsWithinBounds(CameraUtils.Main, bounds, entity.transform.position))
            {
                AddToSelection(entity);
            }
        });
    }

    private void AddSingleEntity()
    {
        Entity entity = SelectUtils.FindEntityAt(CameraUtils.Main, anchor);

        if (entity != null)
        {
            if (entity.Team == level.PlayerTeam)
            {
                AddToSelection(entity);
            }
        }
    }

    private void RemoveAllSelections()
    {

        foreach (Entity entity in Selection)
        {
            //entity.Transform.Find("Halo").gameObject.SetActive(false);
            if (entity == null)
                continue;
            entity.Halo.SetActive(false);
        }

        Selection.Clear();
    }

    public void SelectEntities()
    {
        RemoveAllSelections();

        if (outer == anchor)
        {
            AddSingleEntity();
        }
        else
        {
            AddAllWithinBounds();
        }

        hasActiveBox = false;

        EventManager.Instance.TriggerEvent(new SelectionEvent(Selection));
    }

    public void CreateBoxSelection()
    {

        hasActiveBox = true;

        anchor = Input.mousePosition;
        outer = Input.mousePosition;

        EventManager.Instance.TriggerEvent(new StartSelectBoxEvent(anchor));
    }

    public void DragBoxSelection()
    {
        if (hasActiveBox)
        {
            outer = Input.mousePosition;

            EventManager.Instance.TriggerEvent(new DragSelectBoxEvent(outer));
        }
    }

    public void OnEntityDeathEvent(EntityDeathEvent deathEvent)
    {
        //if (Selection.Contains(deathEvent.Entity))
        Selection.Remove(deathEvent.Entity);
    }

}