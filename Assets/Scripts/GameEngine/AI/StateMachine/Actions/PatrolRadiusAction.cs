using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.AI.FSM
{

    public abstract class PatrolRadiusAction : Action
    {

        [SerializeField]
        protected string usageFlag = "move_to_defend";

        [SerializeField]
        protected string[] ignoreFlags = { "scout" };

        [SerializeField]
        protected int stopMinRange = 7;

        [SerializeField]
        protected int stopMaxRange = 20;

        [SerializeField]
        protected int minSelection = 1;

        [SerializeField]
        protected int slectionRange = 40;

        [SerializeField]
        protected bool ignoreEnemiesOutsideTargetArea = true;

        [SerializeField]
        protected AttackStateFactory.Type attackState = AttackStateFactory.Type.Defensive;

        protected const int PositionMaxTryCount = 50;

        protected void Patrol(StateController controller, Entity target)
        {
            var targetPosition = target.transform.position.Add2(Random.insideUnitCircle.normalized * stopMaxRange);
            NavMeshHit hit;

            NavMesh.Raycast(targetPosition, target.transform.position, out hit, NavMesh.AllAreas);
            targetPosition = hit.position;

            int tryCount = 0;
            while (!NavMesh.SamplePosition(targetPosition, out hit, 20, NavMesh.AllAreas) && tryCount <= stopMaxRange - stopMinRange)
            {
                targetPosition = target.transform.position.Add2(Random.insideUnitCircle.normalized * Random.Range(stopMinRange + tryCount, stopMaxRange));
                NavMesh.Raycast(targetPosition, target.transform.position, out hit, NavMesh.AllAreas);
                targetPosition = hit.position;
                tryCount++;
            }
            if (tryCount > PositionMaxTryCount)
                return;

            List<Entity> selection = new List<Entity>();
            Vector3 selectionMean = Vector3.zero;
            controller.Master.Team.ForEach((ent) =>
            {
                if (ent.GetCachedComponent<IGatherer>() != null)
                    return;

                if (ent.GetCachedComponent<BuildingSpawner>() != null)
                    return;
                var enemy = ent.GetCachedComponent<EntityAttack>().Target;
                if (enemy != null && Vector3.Distance(enemy.transform.position, target.transform.position) <= (stopMinRange + stopMaxRange))
                    return;
                if (!ignoreEnemiesOutsideTargetArea && enemy != null)
                    return;

                var metadata = ent.GetCachedComponent<EntityMetadata>();
                if (metadata.HasFlag(usageFlag + target.transform.position) && ent.GetCachedComponent<IMovable>().WorkingFormation())
                    return;

                foreach (var flag in ignoreFlags)
                {
                    if (metadata.HasFlag(flag))
                        return;
                }
                // var dist = Vector3.Distance(ent.transform.position, target.transform.position);
                //if (dist <= stopMaxRange * 1.5f)
                //return;
                //var sMean = selectionMean;
                if (selection.Count == 0)
                {
                    selectionMean = ent.transform.position;
                    selection.Add(ent);
                }
                else if (Vector3.Distance(selectionMean / selection.Count, ent.transform.position) <= slectionRange)
                {
                    selectionMean += ent.transform.position;
                    selection.Add(ent);
                }

            });
            if (selection.Count < minSelection)
                return;

            foreach (var ent in selection)
            {
                var metadata = ent.GetCachedComponent<EntityMetadata>();
                metadata.WriteFlag(usageFlag + target.transform.position);
                //ent.StopCoroutine("ResetFlag");
                ent.StartCoroutine(ResetFlag(ent, hit.position, usageFlag + target.transform.position));
            }

            MoveCommand comm = new MoveCommand(selection, hit.position);
            comm.Execute();
            MoveEvent mEvent = new MoveEvent(hit.position, selection);
            EventManager.Instance.TriggerEvent(mEvent);

            foreach (var ent in selection)
                ent.GetCachedComponent<EntityAttack>().ChangeState(attackState);

        }

        protected IEnumerator ResetFlag(Entity e, Vector3 target, string tag)
        {
            var dist = Vector3.Distance(e.transform.position, target);
            float time = 1;
            var entAttack = e.GetCachedComponent<EntityAttack>();
            bool enemyCondition = true;
            while (dist > 1 && enemyCondition && time > 0)
            {
                yield return WaitForSecondsCache.Get(0.1f);
                var movable = e.GetCachedComponent<IMovable>();
                if (movable.Formation == null || Vector3.Distance(movable.Formation.Destination, target) > 5 && !movable.WorkingFormation())
                    time -= 0.1f + Time.deltaTime;
                dist = Vector3.Distance(e.transform.position, target);
                var enemy = entAttack.Target;
                enemyCondition = true;
                if (enemy != null && Vector3.Distance(enemy.transform.position, target) <= (stopMinRange + stopMaxRange))
                    enemyCondition = false;

                if (enemy != null && !ignoreEnemiesOutsideTargetArea)
                    enemyCondition = false;
            }

            e.GetCachedComponent<EntityMetadata>().RemoveFlag(tag);
            if (entAttack.Target != null)
            {
                e.GetCachedComponent<IMovable>().Stop();
            }
        }
    }
}
