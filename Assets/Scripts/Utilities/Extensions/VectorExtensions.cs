using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions {

	public static Vector3 Add2 (this Vector3 a, Vector2 b)
    {
        Vector3 c = a;
        c.x += b.x;
        c.z += b.y;
        return c;
    }

}
