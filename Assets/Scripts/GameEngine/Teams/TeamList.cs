using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamList
{

    public Level Level { get; private set; }

    private Team[] teams;
    private TeamListData teamData;

    public TeamList(Level level, TeamListData teamData)
    {
        this.Level = level;
        this.teamData = teamData;

        teams = new Team[teamData.TeamsNumber];
        for (int i = 0; i < teams.Length; i++)
        {
            //Debug.LogWarning("TODO: Change the way color is passed");
            Color c = teamData.GetTeamColor(i);
            teams[i] = new Team(i, "Team" + i, this, c, teamData.IsHumanControlled(i));
        }
    }

    public Team Find(int id)
    {
        return teams[id];
    }

    public Alliance GetAlliance(int id)
    {
        for (int i = 0; i < teamData.Allies.Length; i++)
            if (teamData.Allies[i].IsAllied(id))
                return teamData.Allies[i];

        return null;
    }

    public bool AreAllied(int first, int second)
    {
        return GetAlliance(first).IsAllied(second);
    }

    public bool AreOpponents(int first, int second)
    {
        if (first == 0 || second == 0)
            return false;

        return !AreAllied(first, second);
    }

}
