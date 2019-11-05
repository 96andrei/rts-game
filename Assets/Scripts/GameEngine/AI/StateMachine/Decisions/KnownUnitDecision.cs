using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Known Unit Decision")]
    public class KnownUnitDecision : Decision
    {
        [SerializeField]
        SpawnableConfig lookFor;
        [SerializeField]
        int amount;

        public override bool Decide(StateController controller)
        {
            return Search(controller);
        }

        public bool Search(StateController controller)
        {
            int counter = 0;
            foreach(var e in controller.Master.KnownOpponents)
            {
                if (e.GetCachedComponent<EntityMetadata>().SpawnableConfig == lookFor)
                    counter++;
            }
            return counter >= amount;
            
        }


    }

}
