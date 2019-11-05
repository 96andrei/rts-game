using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Alliance {

    [SerializeField]
    List<int> allies;

    public bool IsAllied(int team)
    {
        if (!allies.Contains(team))
            return false;

        return true;
    }

    public List<int> Members {
        get { return allies; }
    }
         
}
