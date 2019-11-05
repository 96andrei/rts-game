using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Attack Enemy Base Action")]
    public class AttackEnemyBaseAction : PatrolRadiusAction
    {
        public override void Act(StateController controller)
        {
            Entity target = null;
            foreach (var ent in controller.Master.KnownOpponents)
            {
                if (ent == null)
                    continue;

                if (ent.GetCachedComponent<BuildingSpawner>() != null)
                {
                    target = ent;
                    break;
                }
            }
            if (target != null)
                Patrol(controller, target);
        }

    }
}
