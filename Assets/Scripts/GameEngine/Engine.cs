using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine
{

    public Level Level { get; private set; }

    private SelectionController selectionController;
    private CommandController commandController;
    private UiController uiController;
    //private GameOverController _gameOverController;

    public void Update()
    {
        /*if (Level.IsGameOver())
        {
            _gameOverController.EndGame();
            return;
        }*/

        if (!InputInGameArea())
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            selectionController.CreateBoxSelection();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            selectionController.SelectEntities();
        }

        selectionController.DragBoxSelection();

        if (Input.GetButtonDown("Fire2"))
        {
            commandController.ActionAtPoint(Input.mousePosition);
        }

    }

    public bool InputInGameArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(uiController.ControllerHelper.SelectableArea, Input.mousePosition);
    }

    public void LoadLevel(LevelData levelData, int playerId, TeamListData teamData)
    {
        Level = new Level();
        Level.LoadData(levelData, playerId, teamData);

        selectionController = new SelectionController(this.Level);
        commandController = new CommandController(this.Level);
        uiController = new UiController();
        //_gameOverController = new GameOverController(level);

        //SpawnUtils.SpawnAll(level.TeamList);

        EventManager.Instance.AddListener<SelectionEvent>(OnSelection);
        EventManager.Instance.AddListener<StartSelectBoxEvent>(OnStartSelectBox);
        EventManager.Instance.AddListener<DragSelectBoxEvent>(OnDragSelectBox);
        EventManager.Instance.AddListener<MoveEvent>(OnMovement);
        EventManager.Instance.AddListener<AttackEvent>(OnAttack);
        EventManager.Instance.AddListener<GatherEvent>(OnGather);
    }

    private void OnSelection(SelectionEvent e)
    {
        //_commandController.selection = e.selection;

        uiController.ClearBox();
    }

    private void OnStartSelectBox(StartSelectBoxEvent e)
    {
        uiController.StartSelectBox(e.Anchor);
    }

    private void OnDragSelectBox(DragSelectBoxEvent e)
    {
        uiController.DragSelectBox(e.Outer);
    }

    private void OnMovement(MoveEvent e)
    {
        if (e.Entities == null) 
            return;

        if (e.Entities.Count == 0)
            return;

        if (!e.Entities[0].Team.HumanControlled)
            return;

        uiController.OnMovement(e.Point);
    }

    private void OnAttack(AttackEvent e)
    {
        uiController.OnAttack(e.Target);
    }

    private void OnGather(GatherEvent e)
    {
        uiController.OnGather(e.Target.transform.position);
    }

    /*private void OnGameOver(GameOverEvent e)
    {
        _uiController.ClearAll();
    }*/
}