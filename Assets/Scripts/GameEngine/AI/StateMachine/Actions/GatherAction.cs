using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Gather Action")]
    public class GatherAction : Action
    {

        public override void Act(StateController controller)
        {
            GatherResources(controller);
        }

        private void GatherResources(StateController controller)
        {
            if (controller.Master.KnownMines.Count == 0)
                return;

            var mine = controller.Master.FindSafestMine();
            controller.Master.Team.ForEach((e) => { Gather(e, mine); });
        }
        

        private void Gather(Entity e, Entity target)
        {
            EntityGatherer gatherer = e.GetCachedComponent<EntityGatherer>();

            if (gatherer == null)
                return;

            if ((gatherer.Gathering() || gatherer.MovingTo()) && gatherer.Target == target)
                return;

            List<Entity> ent = new List<Entity>();
            ent.Add(e);
            GatherCommand comm = new GatherCommand(ent, target);
            comm.Execute();
            GatherEvent gatherEvent = new GatherEvent(target, ent);
            EventManager.Instance.TriggerEvent(gatherEvent);
        }
    }


}
