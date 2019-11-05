using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    GamePoint gamePoint;
    [SerializeField]
    private LevelData levelData;
    [SerializeField]
    private TeamListData teamData;
    [SerializeField]
    private int playerId;

    public Engine Engine { get; set; }

    private void Awake()
    {
        gamePoint.Game = this;

        Engine = new Engine();

        /*teamData = new TeamListData();
        teamData.TeamsNumber = 4;
        teamData.Allies = new Alliance[3];
        teamData.Allies[0] = new Alliance(0);
        teamData.Allies[1] = new Alliance(1,3);
        teamData.Allies[2] = new Alliance(2);*/

        Engine.LoadLevel(levelData, playerId, teamData);
    }

    private void Update()
    {
        Engine.Update();
    }
}
