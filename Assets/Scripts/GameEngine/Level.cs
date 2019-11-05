using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public TeamList TeamList { get; private set; }

    public int PlayerId { get; private set; }

    public LevelData LevelData { get; private set; }

    public string Name { get; private set; }

    public int width;

    public int length;

    public Level()
    {
    }

    public Level LoadData(LevelData levelData, int playerTeam, TeamListData teamsData)
    {
        this.TeamList = new TeamList(this, teamsData);
        this.LevelData = levelData;
        this.Name = levelData.Name;

        PlayerId = playerTeam;

        return this;
    }

    public Team PlayerTeam {
        get { return TeamList.Find(PlayerId); }
    }

}