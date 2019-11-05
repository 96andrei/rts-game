using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityExtension {

    public static bool Available(this Entity e)
    {
        return e != null && e.enabled;
    }

}
