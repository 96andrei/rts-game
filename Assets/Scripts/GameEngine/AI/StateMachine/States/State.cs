using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{
    [CreateAssetMenu(menuName = "Engine/AI/FSM/State")]
    public class State : ScriptableObject
    {
        [SerializeField]
        private Action[] alwaysActions;
        [SerializeField]
        private Action[] actions;
        [SerializeField]
        private Transition[] transitions;
        public bool HasTransitions { get { return transitions.Length != 0; } }

        public void DoAlwaysActions(StateController controller)
        {
            for (int i = 0; i < alwaysActions.Length; i++)
            {
                alwaysActions[i].Act(controller);
            }
        }
       
        public void DoActions(StateController controller)
        {
            for(int i=0; i<actions.Length; i++)
            {
                actions[i].Act(controller);
            }
        }

        public bool CheckTransitions(StateController controller)
        {
            if(!HasTransitions)
            {
                controller.TransitionToState(controller.FallbackState);
                return false;
            }

            State nextState = null;
            int nextStatePriority = -1;
            for (int i = 0; i < transitions.Length; i++)
            {
                if (transitions[i].priority <= nextStatePriority && nextState != null)
                    continue;

                nextStatePriority = transitions[i].priority;

                bool decisionSucceeded = transitions[i].decision == null ? true : transitions[i].decision.Decide(controller);

                if (decisionSucceeded)
                {
                    if(transitions[i].trueState != null)
                        nextState = transitions[i].trueState;
                }
                else
                {
                    if (transitions[i].falseState != null)
                        nextState = transitions[i].falseState;
                }
            }
            if (nextState != null && nextState != this)
            {
                controller.TransitionToState(nextState);
                return true;
            }

            return false;
        }

    }
}
