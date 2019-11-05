using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleFormation : MoveFormation
{
    public const string Id = "f_rectangle";

    float unitDistance = .15f;

    Vector3 size;

    public RectangleFormation(List<Entity> selection, Transform destination) : base(selection, destination.position)
    {
        unitDrift = 2f;
    }

    public RectangleFormation(List<Entity> selection, Vector3 destination) : base(selection, destination)
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
                if (Vector3.Distance(swap[j].transform.position, nextPosition) < Vector3.Distance(minE.transform.position, nextPosition))
                    minE = swap[j];
            }
            selection[i] = minE;
            swap.Remove(minE);
        }
    }

    public override void PredictUnitPositions()
    {
        unitPositions = new Vector3[selection.Count];

        if (unitPositions.Length == 1)
        {
            unitPositions[0] = Vector3.zero;
            return;
        }

        float lineWidth;

        if (unitPositions.Length <= 3)
        {
            lineWidth = -unitDistance / 2;
            for (int i = 0; i < selection.Count; i++)
            {
                lineWidth += selection[i].GetUnitSize().x + unitDistance / 2;
            }

            for (int i = 0; i < selection.Count; i++)
            {
                unitPositions[i].x = Mathf.Lerp(-lineWidth / 2, lineWidth / 2, i * 1f / (selection.Count - 1));
                unitPositions[i].y = 0;
                unitPositions[i].z = 0;
            }

            return;
        }

        int width = Mathf.CeilToInt(Mathf.Sqrt(selection.Count));
        int depth = Mathf.CeilToInt(selection.Count / width);

        lineWidth = -unitDistance / 2;
        for (int i = 0; i < width; i++)
            lineWidth += selection[i].GetUnitSize().x + unitDistance / 2;

        int lastUnit = 0;

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (lastUnit + 1 > selection.Count - 1)
                    break;

                unitPositions[i * width + j].x = Mathf.Lerp(-lineWidth / 2, lineWidth / 2, j * 1f / (width - 1));
                unitPositions[i * width + j].y = 0;
                unitPositions[i * width + j].z = -(selection[i].GetUnitSize().z + unitDistance) * i;
                lastUnit = i * width + j;
            }
        }

        bool extraLine = lastUnit < selection.Count - 1;

        if (!extraLine)
            return;

        int start = lastUnit + 1;

        int dif = selection.Count - start;

        if (dif == 1)
        {
            unitPositions[start].x = 0;
            unitPositions[start].y = 0;
            unitPositions[start].z = -(selection[start].GetUnitSize().z + unitDistance) * depth; ;// - unitDistance - selection[start].GetUnitSize().z / 2f - selection[lastUnit].GetUnitSize().z / 2f;
            return;
        }

        float extraLineWidth = -unitDistance;

        for (int i = start; i < selection.Count; i++)
            extraLineWidth += selection[i].GetUnitSize().x + unitDistance;

        for (int i = start; i < selection.Count; i++)
        {
            unitPositions[i].x = Mathf.Lerp(-extraLineWidth / 2f, extraLineWidth / 2f, (i - start) * 1f / (dif - 1));
            unitPositions[i].y = 0;
            unitPositions[i].z = -(selection[start].GetUnitSize().z + unitDistance) * depth;// - unitDistance - selection[i].GetUnitSize().z / 2f - selection[lastUnit].GetUnitSize().z / 2f;
        }
    }

    public override Vector3 Size()
    {
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
