using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{

    public class StateController : AiStrategy
    {

        [SerializeField]
        State currentState;
        State nextState;

        [SerializeField]
        State fallbackState;
        public State FallbackState { get { return fallbackState; } }

        private void Awake()
        {
            nextState = currentState;
        }

        public override void UpdateStrategy(AiController master)
        {
            currentState = nextState;

            Master = master;

            currentState.DoAlwaysActions(this);
            if (!currentState.CheckTransitions(this))
                currentState.DoActions(this);
        }

        public void TransitionToState(State nextState)
        {
            this.nextState = nextState;
        }


    }
}
