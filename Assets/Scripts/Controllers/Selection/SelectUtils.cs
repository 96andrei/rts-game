using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUtils
{

    public static Entity FindEntityAt(Camera camera, Vector3 position)
    {
        RaycastHit hit;
        LayerMask mask = 1 << LayerMask.NameToLayer("Entity");

        if (Physics.Raycast(camera.ScreenPointToRay(position), out hit, 1000, mask.value))
           return hit.transform.gameObject.GetComponent<Entity>();

        return null;
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 anchor, Vector3 outer)
    {
        Vector3 anchorView = camera.ScreenToViewportPoint(anchor);
        Vector3 outerView = camera.ScreenToViewportPoint(outer);
        Vector3 min = Vector3.Min(anchorView, outerView);
        Vector3 max = Vector3.Max(anchorView, outerView);

        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static bool IsWithinBounds(Camera camera, Bounds viewportBounds, Vector3 position)
    {
        Vector3 viewportPoint = camera.WorldToViewportPoint(position);

        return viewportBounds.Contains(viewportPoint);
    }

}

