using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI.FSM
{

    [System.Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;
        public int priority;
    }

}