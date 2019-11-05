using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationFactory
{

    private string formationId = RectangleFormation.Id;
    public string FormationId { get { return formationId; } set { formationId = value; } }

    public MoveFormation Get(List<Entity> selection, Vector3 destination)
    {
        var currentFormation = selection[0].GetCachedComponent<IMovable>().Formation;
        int hash = 0;
        if (currentFormation != null)
        {
            hash = currentFormation.GetHashCode();
        }

        switch (FormationId)
        {
            case TravelFormation.Id:
                if (hash == TravelFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new TravelFormation(selection, destination);

            case LineFormation.Id:
                if (hash == LineFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new LineFormation(selection, destination);

            case RectangleFormation.Id:
                if (hash == RectangleFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new RectangleFormation(selection, destination);

        }

        return null;
    }

    public MoveFormation Get(List<Entity> selection, Transform destination)
    {
        var currentFormation = selection[0].GetCachedComponent<IMovable>().Formation;
        int hash = 0;
        if (currentFormation != null)
        {
            hash = currentFormation.GetHashCode();
        }

        switch (FormationId)
        {
            case TravelFormation.Id:
                if (hash == TravelFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new TravelFormation(selection, destination);
            case LineFormation.Id:
                if (hash == LineFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new LineFormation(selection, destination);
            case RectangleFormation.Id:
                if (hash == RectangleFormation.GetHashCode(selection))
                {
                    currentFormation.ChangeDestination(destination);
                    return currentFormation;
                }
                return new RectangleFormation(selection, destination);
        }

        return null;

    }

}
