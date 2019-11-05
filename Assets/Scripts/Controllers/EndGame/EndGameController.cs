using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{

    [SerializeField]
    GameObject panel;

    [SerializeField]
    GameObject[] toDisable;

    [SerializeField]
    Text winnerText;

    [SerializeField]
    int localPlayer = 1;

    private void OnEnable()
    {
        EventManager.Instance.AddListener<EntityDeathEvent>(OnEntityDeath);
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListener<EntityDeathEvent>(OnEntityDeath);

        Time.timeScale = 1;
    }

    void OnEntityDeath(EntityDeathEvent e)
    {
        if (e.Entity.GetCachedComponent<EntityMetadata>().Type.TypeId != "e_building")
            return;

        if (e.Entity.Team.Id == localPlayer)
            winnerText.text = "AI Wins";
        else
            winnerText.text = "Player Wins";

        panel.SetActive(true);
        foreach (var item in toDisable)
            item.SetActive(false);

        Time.timeScale = 0;
    }

    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
