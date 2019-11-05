using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Engine/GameInfo/New TeamData")]
public class TeamListData : ScriptableObject {

    [SerializeField]
    int teamsNumber;

    [SerializeField]
    List<int> humanControlled;

    [SerializeField]
    Alliance[] allies;

    [SerializeField]
    Color[] colors;


	public int TeamsNumber { get { return teamsNumber; } }
    public Alliance[] Allies { get { return allies; } }
    public bool IsHumanControlled(int teamId)
    {
        return humanControlled.Contains(teamId);
    }

    public Color GetTeamColor(int teamId)
    {
        return colors[teamId];
    }

}
