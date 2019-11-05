using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI
{

    public class AiController : MonoBehaviour
    {

        [SerializeField]
        private GamePoint gamePoint;
        [SerializeField]
        private int teamId = 2;
        [SerializeField]
        private InfluenceMap influenceMap;
        [Header("Strategies")]
        [SerializeField]
        float updateInterval = 0.1f;
        float currentUpdateInterval = 0;
        [SerializeField]
        private AiStrategy buildStrategy;
        [SerializeField]
        private AiStrategy unitStrategy;
        private BuildingSpawner teamSpawner;
        public Entity town;

        public HashSet<Entity> KnownOpponents = new HashSet<Entity>();
        public List<Entity> KnownMines;
        public InfluenceMap InfluenceMap { get { return influenceMap; } }
        public Team Team { get; private set; }
        public BuildingSpawner TeamSpawner {
            get {
                if (teamSpawner != null)
                    return teamSpawner;

                Team.ForEach((ent) =>
                {
                    if (teamSpawner == null)
                    {
                        teamSpawner = ent.GetCachedComponent<BuildingSpawner>();
                        town = ent;
                    }
                });
                return teamSpawner;
            }
        }
        public Entity Town {
            get {
                if (town != null)
                    return town;

                Team.ForEach((ent) =>
                {
                    if (teamSpawner == null)
                    {
                        teamSpawner = ent.GetCachedComponent<BuildingSpawner>();
                        town = ent;
                    }
                });
                return town;
            }
        }

        //cached components to avoid new allocations
        HashSet<Entity> nearbyEntities = new HashSet<Entity>();
        NavMeshPath path;
        bool safestMineThisFrame = false;
        Entity safestMine = null;

        private void Start()
        {
            Team = gamePoint.Game.Engine.Level.TeamList.Find(teamId);
        }

        private void Update()
        {
            safestMineThisFrame = false;

            if (TeamSpawner == null)
                return;

            //check wheter it is time to update the Ai State or not
            currentUpdateInterval += Time.deltaTime;
            if (currentUpdateInterval < updateInterval)
                return; //not the time
            //time to update

            //reset update interval
            currentUpdateInterval = 0;

            CheckNearbyEntities();

            if (buildStrategy != null)
                buildStrategy.UpdateStrategy(this);

            if (unitStrategy != null)
                unitStrategy.UpdateStrategy(this);
        }

        public Entity FindSafestMine()
        {
            if (safestMineThisFrame)
                return safestMine;

            if (path == null)
                path = new NavMeshPath();

            Entity mine = null;
            Vector3 basePosition = TeamSpawner.transform.position;
            NavMeshHit baseHit;
            NavMesh.SamplePosition(basePosition, out baseHit, 10, NavMesh.AllAreas);
            float minDifficulty = float.MaxValue;
            foreach (var m in KnownMines)
            {
                NavMeshHit mineHit;
                NavMesh.SamplePosition(m.transform.position, out mineHit, 10, NavMesh.AllAreas);

                NavMesh.CalculatePath(baseHit.position, mineHit.position, NavMesh.AllAreas, path);
                float difficulty = 0;
                for (int i = 1; i < path.corners.Length; i++)
                    difficulty += Vector3.Distance(path.corners[i - 1], path.corners[i]);

                var gridPos = InfluenceMap.WorldToGridPosition(m.transform.position);
                int searchRange = 6;
                for (int i = gridPos.x - searchRange; i <= gridPos.x + searchRange; i++)
                    for (int j = gridPos.y - searchRange; j <= gridPos.y + searchRange; j++)
                    {
                        var influencer = InfluenceMap.GetTile(i, j);
                        var inf = influencer.GetInfluence(InfluenceType.ZoneControl);
                        float multiplier = -4;
                        if (inf < 0)
                            difficulty += inf * multiplier;
                    }
                if (difficulty < minDifficulty)
                {
                    minDifficulty = difficulty;
                    mine = m;
                }
            }

            safestMineThisFrame = true;
            safestMine = mine;

            return mine;
        }

        public void CheckNearbyEntities()
        {
            //create a list of all the enemy/neutral entities in range
            nearbyEntities.Clear();
            Team.ForEach((ent) =>
            {
                var results = Physics.OverlapSphere(ent.transform.position, ent.VisibilityRange);
                foreach (var c in results)
                {
                    var target = c.gameObject.GetCachedEntity();
                    if (target == null) //check if the found object is an entity
                        continue;

                    if (target.Team != null && Team.TeamList.AreAllied(target.Team.Id, teamId)) //ignore same team entities
                        continue;

                    nearbyEntities.Add(target);
                }
            });

            foreach (var ent in nearbyEntities)
            {
                if (ent.Team != null && Team.TeamList.AreOpponents(teamId, ent.Team.Id))
                {
                    if (KnownOpponents.Add(ent))
                    {
                        var finalE = ent;
                        ent.OnEntityDeath += () => KnownOpponents.Remove(finalE);
                    }
                    continue;
                }

                var gatherable = ent.GetCachedComponent<EntityGatherable>();
                if (gatherable != null && !gatherable.Empty() && !KnownMines.Contains(ent))
                {
                    KnownMines.Add(ent);
                    gatherable.OnUnitTaken += () => { if (gatherable.Empty()) KnownMines.Remove(gatherable.gameObject.GetCachedEntity()); };
                    continue;
                }

            }

        }
    }
}
