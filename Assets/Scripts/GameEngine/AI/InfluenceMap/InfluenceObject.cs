using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceObject : MonoBehaviour {

    [SerializeField]
    protected InfluenceType type;
    public InfluenceType Type { get { return type; } }

    [SerializeField]
    protected float strength = 10;
    public float Strength { get { return strength; } set { strength = value; } }

    [SerializeField]
    protected float multiplier = 1;
    public float Multiplier { get { return multiplier; } set { multiplier = value; } }

    public float Influence { get { return strength * multiplier; } }

    [SerializeField]
    protected float decrease = 1;
    public float Decrease { get { return decrease; } set { decrease = value; } }

    [SerializeField]
    protected int radius = 5;
    public int Radius { get { return radius; } set { radius = value; } }
}
