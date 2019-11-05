using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Create Unit Decision")]
    public class UnitCountDecision : Decision
    {
        [SerializeField]
        private UnitValue[] expectedUnits;

        public override bool Decide(StateController controller)
        {
            return CheckUnitCount(controller);
        }

        private bool CheckUnitCount(StateController controller)
        {
            for (int i = 0; i < expectedUnits.Length; i++)
            {
                int count = 0;
                controller.Master.Team.ForEach((ent) =>
                {
                    if (ent.GetCachedComponent<EntityMetadata>().SpawnableConfig == expectedUnits[i].Unit)
                        count++;
                });

                var spawner = controller.Master.TeamSpawner;
                if (spawner != null)
                    foreach (var t in spawner.TaskQueue)
                        if (t.Config == expectedUnits[i].Unit)
                            count++;

                if (count < expectedUnits[i].Value)
                    return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class UnitValue
    {
        public SpawnableConfig Unit;
        public int Value;
    }

}
