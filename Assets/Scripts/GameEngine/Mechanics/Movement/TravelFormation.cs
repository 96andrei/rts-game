using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelFormation : MoveFormation {

    public const string Id = "f_travel";

    float unitDistance = 0.15f;

    public TravelFormation(List<Entity> selection, Transform destination) : base(selection, destination)
    {
        unitDrift = 2f;
    }

    public TravelFormation(List<Entity> selection, Vector3 destination) : base(selection, destination)
    {
        unitDrift = 2f; 
    }

    public override void AssignUnitPositions()
    {
        List<Entity> swap = new List<Entity>(selection);

        for (int i = 0; i < unitPositions.Length; i++)
        {
            Entity minE = swap[0];
            Vector3 nextPosition = leader.GetTargetPosition(unitPositions[i]);
            for (int j = 1; j < swap.Count; j++)
            {
                if (Vector3.Distance(minE.transform.position, nextPosition) > Vector3.Distance(swap[j].transform.position, nextPosition))
                    minE = swap[j];
            }
            selection[i] = minE;
            swap.Remove(minE);
        }
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

    public override void PredictUnitPositions()
    {
        unitPositions = new Vector3[selection.Count];

        if(unitPositions.Length == 1)
        {
            unitPositions[0] = Vector3.zero;
            return;
        }

        unitPositions[0] = new Vector3(-(selection[0].GetUnitSize().x / 2f + unitDistance / 2f), 0, 0);
        if (unitPositions.Length > 1)
            unitPositions[1] = new Vector3(selection[1].GetUnitSize().x / 2f + unitDistance / 2f, 0, 0);

        int sign = 1;

        for (int i = 2; i < selection.Count; i++)
        {
            int previous = i - 2;
            previous = Mathf.Clamp(previous, 0, selection.Count);
            unitPositions[i] = unitPositions[previous];

            unitPositions[i].x = sign * (selection[i].GetUnitSize().x / 2f + unitDistance / 2f);
            unitPositions[i].z -= selection[i].GetUnitSize().z / 2f + unitDistance + selection[previous].GetUnitSize().z;
            unitPositions[i].y = 0;

            sign *= -1;
        }
    }

    public override Vector3 Size()
    {
        Vector3 size = Vector3.zero;

        foreach (var e in selection)
        {
            size += e.GetUnitSize();
            size.x += unitDistance;
        }
        size.x -= unitDistance;

        return size;
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
