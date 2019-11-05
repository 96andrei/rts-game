using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Defend Mine Action")]
    public class DefendMineAction : PatrolRadiusAction
    {
        public override void Act(StateController controller)
        {
            var mine = controller.Master.FindSafestMine();
            Patrol(controller, mine);
        }

    }
}
