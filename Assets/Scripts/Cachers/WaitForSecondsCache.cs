using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitForSecondsCache {

    private static Dictionary<float, WaitForSeconds> cache;

	public static WaitForSeconds Get(float time)
    {

        if (cache == null)
            cache = new Dictionary<float, WaitForSeconds>();

        WaitForSeconds wait;
        if (cache.TryGetValue(time, out wait))
            return wait;

        wait = new WaitForSeconds(time);
        cache[time] = wait;

        return wait;

    }
}
