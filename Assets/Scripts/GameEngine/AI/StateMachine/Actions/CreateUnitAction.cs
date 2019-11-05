using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Create Unit Action")]
    public class CreateUnitAction : Action
    {

        [SerializeField]
        SpawnableConfig unit;

        public override void Act(StateController controller)
        {
            CreateUnit(controller);
        }

        private void CreateUnit(StateController controller)
        {
            var spawner = controller.Master.TeamSpawner;
            if (spawner == null)
                return;
            
            if(controller.Master.Team.CanAfford(unit.Cost))
                spawner.Enqueue(unit, controller.Master.Team.Id);
        }
    }
}
