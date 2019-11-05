using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceGatherableMultiplier : InfluenceMultiplier
{
    [SerializeField]
    EntityGatherable gatherable;

    [SerializeField]
    AnimationCurve curve;

    private void OnEnable()
    {
        gatherable.OnUnitTaken += ChangeInfluence;
    }

    private void OnDisable()
    {
        gatherable.OnUnitTaken -= ChangeInfluence;
    }

    public override void ChangeInfluence()
    {
        influencer.Multiplier = curve.Evaluate(gatherable.Units * 1f / gatherable.Stats.Units);
    }
}
