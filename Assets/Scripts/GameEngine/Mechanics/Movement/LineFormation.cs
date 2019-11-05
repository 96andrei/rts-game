using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFormation : MoveFormation {

    public const string Id = "f_line";

    float unitDistance = .15f;

    public LineFormation(List<Entity> selection, Transform destination) : base(selection, destination)
    {
        unitDrift = 2f;
    }

    public LineFormation(List<Entity> selection, Vector3 destination) : base(selection, destination)
    {
        unitDrift = 2f;
    }

    public override void PredictUnitPositions()
    {
        unitPositions = new Vector3[selection.Count];

        if (unitPositions.Length == 1)
        {
            unitPositions[0] = Vector3.zero;
            return;
        }

        float lineWidth = -unitDistance / 2;

        for (int i = 0; i < selection.Count; i++)
        {
            lineWidth += unitDistance / 2 + selection[i].GetUnitSize().x;
        }

        for (int i = 0; i < selection.Count; i++)
        {
            unitPositions[i].x = Mathf.Lerp(-lineWidth / 2, lineWidth / 2, i * 1f / (selection.Count-1));
            unitPositions[i].y = 0;
            unitPositions[i].z = 0;
        }

    }

    public override void AssignUnitPositions()
    {
        //start ordering units from the middle of the formation
        List<Entity> swap = new List<Entity>(selection);

        int nextItem = unitPositions.Length / 2;
        for (int i = 1; i <= unitPositions.Length; i++)
        {
            Entity minE = swap[0];
            for (int j = 1; j < swap.Count; j++)
            {
                if (Vector3.Distance(minE.transform.position, 
                    leader.GetTargetPosition(unitPositions[nextItem])) >=
                Vector3.Distance(swap[j].transform.position, leader.GetTargetPosition(unitPositions[nextItem])))
                    minE = swap[j];
            }
            selection[nextItem] = minE;
            swap.Remove(minE);
            nextItem += i % 2 == 0 ? i : -i;
        }

    }

    public override Vector3 Size()
    {
        Vector3 size = Vector3.zero;

        foreach(var e in selection)
        {
            size += e.GetUnitSize();
            size.x += unitDistance;
        }
        size.x -= unitDistance;

        return size;
    }

    public override int GetHashCode()
    {
        int hash = Id.GetHashCode();
        foreach (Entity e in selection)
        {
            hash += e.GetHashCode();
        }
        return hash;
    }

    public static int GetHashCode(List<Entity> selection)
    {
        int hash = Id.GetHashCode();
        foreach (Entity e in selection)
        {
            hash += e.GetHashCode();
        }
        return hash;
    }

}

