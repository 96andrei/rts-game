using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfluenceMultiplier : MonoBehaviour {

    [SerializeField]
    protected InfluenceObject influencer;

    public abstract void ChangeInfluence();
}
