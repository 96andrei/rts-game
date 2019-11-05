using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandController
{
    private Level level;
    private List<Entity> selection;
    LayerMask mask;

    public CommandController(Level level)
    {
        this.level = level;
        mask = 1 << LayerMask.NameToLayer("Terrain");
        mask = 1 << LayerMask.NameToLayer("Entity");
        EventManager.Instance.AddListener<SelectionEvent>(OnSelection);
    }

    public void ActionAtPoint(Vector3 point)
    {
        if (selection == null || selection.Count == 0)
            return;

        RaycastHit hit;
        if(Physics.Raycast(CameraUtils.Main.ScreenPointToRay(point), out hit, mask))
        {
            ITargetable target = hit.transform.gameObject.GetComponent<ITargetable>();

            ICommand command = null;

            if(target != null)
                command = target.Resolve(selection.ToList(), hit.point);

            if (command != null)
            {
                command.Execute();
                return;
            }

            MoveCommand move = new MoveCommand(selection.ToList(), hit.point);
            move.Execute();

            return;
        }

    }

    void OnSelection(SelectionEvent e)
    {
        if (e.StaticObject)
        {
            selection = null;
            return;
        }

        selection = e.Selection;
    }


}