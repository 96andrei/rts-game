using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : Spawner {

    [SerializeField]
    BuildingConfiguration configuration;
    public BuildingConfiguration Configuration { get { return configuration; } }

    Queue<SpawnTask> taskQueue = new Queue<SpawnTask>();
    public Queue<SpawnTask> TaskQueue { get { return taskQueue; } }

    int unitCounter = 0;

    public class SpawnTask
    {
        public SpawnableConfig Config;
        public int Team;
        public float Progress;
    }

    public void Enqueue(SpawnableConfig cfg, int teamId)
    {
        //Debug.LogWarning("TODO: Constant value");
        //Debug.LogWarning("TODO: Raise event");

        var team = gamePoint.Game.Engine.Level.TeamList.Find(teamId);

        //TODO raise event to notify the ui?
        if (!team.CanAfford(cfg.Cost))
            return;

        if (taskQueue.Count >= 11)
            return;

        SpawnTask task = new SpawnTask();
        task.Config = cfg;
        task.Team = teamId;
        task.Progress = 0;

        team.UseGold(cfg.Cost);
        taskQueue.Enqueue(task);
    }

    public override void Spawn(SpawnableConfig spawn, int team)
    {
        //Debug.LogWarning("OPTIMIZATION: Instantiating object. Consider pooling objects.");
        GameObject go = Instantiate(spawn.Prefab, null);
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(transform.position, out hit, 50, 1);
        go.transform.position = hit.position;
        go.transform.rotation = transform.rotation;
        Entity e = go.GetComponent<Entity>();
        gamePoint.Game.Engine.Level.TeamList.Find(team).AddUnit(e);
        go.name += unitCounter;
        go.SetActive(true);
        //e.GetCachedComponent<IMovable>().Move(hit.position, FormationFactory.Get(new List<Entity> { e }, transform.forward));
        unitCounter++;
    }

    private void Update()
    {
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (taskQueue.Count == 0)
            return;

        var task = taskQueue.Peek();
        task.Progress += Time.deltaTime / task.Config.Duration;

        if (taskQueue.Peek().Progress >= 1)
        {
            task = taskQueue.Dequeue();
            Spawn(task.Config, task.Team);
        }

    }

}
