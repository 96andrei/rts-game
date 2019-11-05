using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingMenu : MonoBehaviour {

    [SerializeField]
    EntityType entityType;

    [SerializeField]
    int localTeam;

    [SerializeField]
    GameObject panel;

    [SerializeField]
    GameObject holdingPanel;

    [SerializeField]
    Button[] buttons;

    [SerializeField]
    Image[] queueImages;

    [SerializeField]
    Image progressBar;

    [SerializeField]
    Image currentTaskImage;
    /***************
    TODO Display production queue
    ***************/

    private BuildingSpawner.SpawnTask currentTask;
    private BuildingSpawner spawner;

    void OnEnable()
    {
        EventManager.Instance.AddListener<SelectionEvent>(OnSelection);
    }

    void OnSelection(SelectionEvent e)
    {
        ClosePanel();

        var ent = e.Selection;
        if (ent == null || ent.Count == 0)
            return;

        if (ent[0].Team.Id != localTeam)
            return;

        if (ent[0].GetCachedComponent<EntityMetadata>().Type != entityType)
            return;

        spawner = ent[0].GetCachedComponent<BuildingSpawner>();

        if (spawner == null)
        {
            ClosePanel();
            return;
        }

        int index = 0;
        foreach(var t in spawner.Configuration.Tiers)
        {
            foreach(var item in t.Items)
            {
                buttons[index].GetComponentInChildren<Text>().text = item.Cost.ToString();
                buttons[index].image.sprite = item.UiData.Sprite;
                buttons[index].onClick.RemoveAllListeners();
                buttons[index].onClick.AddListener(()=>
                {
                    spawner.Enqueue(item, localTeam);
                });
                buttons[index].gameObject.SetActive(true);
                index++;
            }
        }

        UpdateQueueVisuals();

        holdingPanel.SetActive(true);
        panel.SetActive(true);
    }

    private void UpdateQueueVisuals()
    {
        foreach (var i in queueImages)
            i.gameObject.SetActive(false);

        currentTaskImage.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);

        if (spawner == null)
            return;

        int index = 0;
        var taskQueue = spawner.TaskQueue;
        if (taskQueue.Count > 0)
        {
            currentTaskImage.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);
        }

        foreach (var t in taskQueue)
        {
            if (index == 0)
            {
                currentTask = t;
                currentTaskImage.sprite = t.Config.UiData.Sprite;
            }
            else
            {
                queueImages[index - 1].sprite = t.Config.UiData.Sprite;
                queueImages[index - 1].gameObject.SetActive(true);
            }

            index++;
        }
    }

    private void Update()
    {
        UpdateQueueVisuals();

        if (currentTask == null)
            return;

        progressBar.fillAmount = currentTask.Progress;
        if (currentTask.Progress >= 1)
        {
            currentTask = null;
        }
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        foreach (var b in buttons)
        {
            b.onClick.RemoveAllListeners();
            b.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if(EventManager.Instance != null)
            EventManager.Instance.RemoveListener<SelectionEvent>(OnSelection);
    }

}
