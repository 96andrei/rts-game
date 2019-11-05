using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Create Weighted Units Action")]
    public class CreateWeightedUnitsAction : Action
    {
        [SerializeField]
        WeightedUnit[] units;

        SpawnableConfig toSpawnConfig;

        Dictionary<int, float> unitCount = new Dictionary<int, float>();

        public override void Act(StateController controller)
        {
            var spawner = controller.Master.TeamSpawner;
            if (spawner == null)
                return;

            unitCount.Clear();

            float totalWeight = 0;
            int totalCount = 0;
            for (int i = 0; i < units.Length; i++)
            {
                int count = 0;
                controller.Master.Team.ForEach((ent) =>
                {
                    if (ent.GetCachedComponent<EntityMetadata>().SpawnableConfig == units[i].Config)
                        count++;
                });
                unitCount[i] = count;
                totalCount += count;
                totalWeight += units[i].Weight;
            }

            float maxError = 0;
            int maxIndex = 0;
            for (int i = 0; i < units.Length; i++)
            {
                float error = units[i].Weight / totalWeight - unitCount[i] * 1f / totalCount;
                if (error > maxError)
                {
                    maxError = error;
                    maxIndex = i;
                }

            }

            toSpawnConfig = units[maxIndex].Config;
            CreateUnit(controller);
        }

        private void CreateUnit(StateController controller)
        {
            var spawner = controller.Master.TeamSpawner;
            if (controller.Master.Team.CanAfford(toSpawnConfig.Cost))
                spawner.Enqueue(toSpawnConfig, controller.Master.Team.Id);
        }

    }

    [System.Serializable]
    public struct WeightedUnit
    {
        public SpawnableConfig Config;
        public float Weight;
    }
}
