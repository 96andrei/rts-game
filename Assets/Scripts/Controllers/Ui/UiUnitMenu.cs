using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiUnitMenu : MonoBehaviour
{

    [SerializeField]
    GamePoint gamePoint;

    [SerializeField]
    EntityType entityType;

    [SerializeField]
    int localTeam = 1;

    [SerializeField]
    List<Button> buttons;

    [SerializeField]
    GameObject panel;

    [SerializeField]
    GameObject stanceArea;

    [SerializeField]
    GameObject formationArea;

    [SerializeField]
    GameObject selectionArea;

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

        for(int i=0; i < e.Selection.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button b = Instantiate(buttons[0], selectionArea.transform, false).GetComponent<Button>();
                buttons.Add(b);
            }

            buttons[i].image.sprite = e.Selection[i].GetCachedComponent<EntityMetadata>().UiData.Sprite;
            buttons[i].gameObject.SetActive(true);

        }

        panel.SetActive(true);

    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        foreach (var b in buttons)
        {
            b.onClick.RemoveAllListeners();
            b.gameObject.SetActive(false);
        }
    }

    public void SetFormation(string id)
    {
        var factory = gamePoint.Game.Engine.Level.TeamList.Find(localTeam).FormationFactory;
        factory.FormationId = id;
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.RemoveListener<SelectionEvent>(OnSelection);
    }
}