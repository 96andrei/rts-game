using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI.FSM
{

    [CreateAssetMenu(menuName = "Engine/AI/FSM/Actions/Scout Action")]
    public class ScoutAction : Action
    {
        [SerializeField]
        UnitValue[] units;

        [SerializeField]
        float updateThreshold = 60;

        [SerializeField]
        string usageFlag = "scout";

        List<Entity> foundUnits = new List<Entity>();


        public override void Act(StateController controller)
        {
            Scout(controller);
        }

        private void Scout(StateController controller)
        {
            //foundUnits.Clear();
            foundUnits = new List<Entity>();
            for (int i = 0; i < units.Length; i++)
            {
                int count = 0;
                controller.Master.Team.ForEach((ent) =>
                {
                    if (count >= units[i].Value)
                        return;

                    var md = ent.GetCachedComponent<EntityMetadata>();
                    if (md.HasFlag(usageFlag))
                    {
                        count++;
                        return;
                    }
                    if (md.SpawnableConfig == units[i].Unit)
                    {
                        if (count < units[i].Value)
                        {
                            foundUnits.Add(ent);
                            count++;
                        }
                    }
                });
            }

            var im = controller.Master.InfluenceMap;
            int x = Random.Range(0, im.Width);
            int y = Random.Range(0, im.Height);

            if (Time.timeSinceLevelLoad - im.GetTile(x, y).LastUpdate < updateThreshold)
            {
                Scout(controller);
                return;
            }

            foreach (var u in foundUnits)
            {
                var md = u.GetCachedComponent<EntityMetadata>();
                if (md.HasFlag(usageFlag))
                    continue;
                md.WriteFlag(usageFlag);
                var pos = im.GridToWorldPosition(x, y);
                MoveCommand command = new MoveCommand(foundUnits, pos);
                command.Execute();
                MoveEvent mEvent = new MoveEvent(pos, foundUnits);
                EventManager.Instance.TriggerEvent(mEvent);
                u.StartCoroutine(ResetFlag(md, u, 60, im.GetTile(x, y)));
            }
        }

        public IEnumerator ResetFlag(EntityMetadata md, Entity e, float time, InfluenceTile tile)
        {
            float elapsedTime = 0;

            while (elapsedTime < time && Time.timeSinceLevelLoad - tile.LastUpdate >= updateThreshold)
            {
                if (!e.GetCachedComponent<IMovable>().WorkingFormation()) {
                    md.RemoveFlag(usageFlag);
                    yield break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            md.RemoveFlag(usageFlag);
        }
    }
}
