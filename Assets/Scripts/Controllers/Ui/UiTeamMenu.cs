using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTeamMenu : MonoBehaviour
{

    [SerializeField]
    Game game;
    [SerializeField]
    int teamId;

    [SerializeField]
    Text goldText;
    [SerializeField]
    Text unitsCountText;

    Team team;

    void Start()
    {
        team = game.Engine.Level.TeamList.Find(teamId);
        team.OnTeamStateChanged += Team_OnTeamStateChanged;
    }

    private void Team_OnTeamStateChanged()
    {
        goldText.text = team.Gold.ToString();
        unitsCountText.text = team.UnitCount().ToString();
    }

    private void OnDestroy()
    {
        team.OnTeamStateChanged -= Team_OnTeamStateChanged;
    }
}
