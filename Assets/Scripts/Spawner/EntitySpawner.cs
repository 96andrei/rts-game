using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntitySpawner : Spawner {

    [SerializeField] int team;
    [SerializeField] bool spawnOnStart;
    [SerializeField] SpawnableConfig toSpawn;

    private void Start()
    {
        if (spawnOnStart)
            Spawn(toSpawn, team);
    }

    public override void Spawn(SpawnableConfig spawn, int team)
    {
        //Debug.LogWarning("OPTIMIZATION: Instantiating object. Consider pooling objects.");
        GameObject go = Instantiate(spawn.Prefab, null);
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 50, 1);
        go.transform.position = hit.position;
        go.transform.rotation = transform.rotation;
        if (team == 2)
        {
            //go.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            go.name += " Enemy";
        }
        Entity e = go.GetComponent<Entity>();
        gamePoint.Game.Engine.Level.TeamList.Find(team).AddUnit(e);
        go.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }

}
