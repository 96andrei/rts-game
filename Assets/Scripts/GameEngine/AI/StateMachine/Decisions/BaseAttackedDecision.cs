using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Create Base Attacked Decision")]
    public class BaseAttackedDecision : Decision
    {
        [SerializeField]
        protected float searchRadius = 40;

        public override bool Decide(StateController controller)
        {
            return SearchRange(controller);
        }

        protected bool SearchRange(StateController controller)
        {
            var town = controller.Master.TeamSpawner;

            var collider = Physics.OverlapSphere(town.transform.position, searchRadius);

            foreach(var c in collider)
            {
                var ent = c.gameObject.GetCachedEntity();
                if (ent == null)
                    continue;

                if (ent.Team == null)
                    continue;

                if (ent.Team.TeamList.AreOpponents(ent.Team.Id, controller.Master.Team.Id))
                    return true;
            }

            return false;
        }
    }
}