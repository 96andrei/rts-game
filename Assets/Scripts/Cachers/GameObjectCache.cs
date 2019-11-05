using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectCache {

    private static Dictionary<GameObject, Entity> cache = new Dictionary<GameObject, Entity>();

    public static Entity GetCachedEntity(this GameObject g){

        Entity ent;

        if(cache.TryGetValue(g, out ent))
        {
            return ent;
        }

        ent = g.GetComponent<Entity>();
        cache[g] = ent;

        return ent;

    }

}
