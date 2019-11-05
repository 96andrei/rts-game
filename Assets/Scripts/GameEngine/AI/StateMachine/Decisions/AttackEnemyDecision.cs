using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Create Attack Decision")]
    public class AttackEnemyDecision : CounterEnemyDecision
    {

        [SerializeField]
        SpawnableConfig target;
        [SerializeField]
        int amount;

        public override bool Decide(StateController controller)
        {
            return Counter(controller) && Search(controller);
        }

        public bool Search(StateController controller)
        {
            int counter = 0;
            foreach (var e in controller.Master.KnownOpponents)
            {
                if (e.GetCachedComponent<EntityMetadata>().SpawnableConfig == target)
                    counter++;
            }
            return counter >= amount;

        }
    }
}