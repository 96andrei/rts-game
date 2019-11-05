using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Engine/AI/Influence Bridge")]
public class InfluenceBridge : ScriptableObject {

    Dictionary<int, InfluenceMap> maps = new Dictionary<int, InfluenceMap>();

    public void RegisterMap(int team, InfluenceMap map)
    {
        maps[team] = map;
    }

    public InfluenceMap GetMap(int team)
    {
        return maps[team];
    }
	
}
