using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfluenceScanner : MonoBehaviour
{

    [SerializeField]
    GamePoint gamePoint;

    [SerializeField]
    int teamId;

    Team team;

    // Use this for initialization
    void Start()
    {
        team = gamePoint.Game.Engine.Level.TeamList.Find(teamId);
        if (team.HumanControlled)
            enabled = false;
    }

    public void Scan(InfluenceMap map)
    {
        team.ForEach((ent) =>
        {
            if (ent == null)
                return;

            var results = Physics.OverlapSphere(ent.transform.position, ent.VisibilityRange);
            foreach (var c in results)
            {
                var target = c.gameObject.GetCachedEntity();
                if (target == null)
                    continue;
                var influencer = target.GetCachedComponent<InfluenceObject>();
                if (influencer != null)
                    map.AddInfluencer(influencer);
            }
        });
    }

}
