using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Defend Town Action")]
    public class DefendTownAction: PatrolRadiusAction
    {
        public override void Act(StateController controller)
        {
            var town = controller.Master.Town;
            Patrol(controller, town);
        }

    }
}
