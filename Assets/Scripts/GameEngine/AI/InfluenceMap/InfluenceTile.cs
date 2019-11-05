using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceTile {

    public int X { get; set; }
    public int Y { get; set; }

    public float LastUpdate { get; private set; }
    public float TimeSinceLastUpdate { get { return Time.timeSinceLevelLoad - LastUpdate; } }

    public Vector3 WorldPosition { get; set; }

    private List<InfluenceTile> neighbours = new List<InfluenceTile>();

    private Dictionary<InfluenceType, float> influences = new Dictionary<InfluenceType, float>();

    public InfluenceTile()
    {
        LastUpdate = -10000;
    }

    public void SetInfluece(InfluenceType type, float value)
    {
        influences[type] = value;
        LastUpdate = Time.timeSinceLevelLoad;
    }

    public void AddInfluence(InfluenceType type, float value)
    {
        float influence = 0;
        influences.TryGetValue(type, out influence);
        influence += value;
        influences[type] = influence;
        LastUpdate = Time.timeSinceLevelLoad;
    }

    public float GetInfluence(InfluenceType type)
    {
        float influence = 0;
        influences.TryGetValue(type, out influence);
        influences[type] = influence;
        return influence;
    }

    public void AddNeighbour(InfluenceTile n)
    {
        if(!neighbours.Contains(n))
            neighbours.Add(n);
    }

    public List<InfluenceTile> GetNeighbours()
    {
        return neighbours;
    }

}
