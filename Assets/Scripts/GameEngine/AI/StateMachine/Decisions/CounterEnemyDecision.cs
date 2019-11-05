using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Create Counter Decision")]
    public class CounterEnemyDecision : Decision
    {
        [SerializeField]
        protected SpawnableConfig[] lookFor;
        [SerializeField]
        protected SpawnableConfig[] counterWith;
        [SerializeField]
        protected float counterValue = 1.5f;

        public override bool Decide(StateController controller)
        {
            return Counter(controller);
        }

        public bool Counter(StateController controller)
        {

            for (int i = 0; i < lookFor.Length; i++)
            {
                float opponentCount = 0;
                foreach (var ent in controller.Master.KnownOpponents)
                {
                    if (ent == null)
                        continue;

                    if (ent.GetCachedComponent<EntityMetadata>().SpawnableConfig == lookFor[i])
                        opponentCount++;
                }

                float counterCount = 0;
                controller.Master.Team.ForEach((ent) =>
                {
                    if (ent.GetCachedComponent<EntityMetadata>().SpawnableConfig == counterWith[i])
                        counterCount += counterValue;
                    if (ent.GetCachedComponent<EntityMetadata>().SpawnableConfig == lookFor[i])
                        counterCount += 0.5f;
                });

                if (counterCount < opponentCount)
                    return false;
            }

            return true;
        }

    }

}
