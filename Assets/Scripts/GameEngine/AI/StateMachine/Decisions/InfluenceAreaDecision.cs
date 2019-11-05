using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/Decisions/Create Influence Area Decision")]
    public class InfluenceAreaDecision : Decision
    {
        [SerializeField]
        InfluenceType influenceType;

        [SerializeField]
        [Range(0, 1)]
        float threshold;

        [SerializeField]
        int step = 1;


        public override bool Decide(StateController controller)
        {
            return CheckInfluence(controller);
        }

        private bool CheckInfluence(StateController controller)
        {
            var im = controller.Master.InfluenceMap;

            if (!im.UpdatedThisFrame)
                return false;

            int opponentSquares = 0;

                for (int x = 0; x <= im.Width; x+=step)
                {
                    for (int y = 0; y <= im.Height; y += step)
                    {
                        if (!im.IsInGrid(y, x))
                            continue;

                        var tile = im.GetTile(x, y);
                        if (tile.GetInfluence(influenceType) < 0)
                            opponentSquares++;
                    }
                }
            

            return (opponentSquares * 1f / (im.Width * im.Height)) >= threshold;

        }
    }
}