using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.AI
{
    public abstract class AiStrategy : MonoBehaviour
    {

        public AiController Master { get; protected set; }
        public abstract void UpdateStrategy(AiController master);

    }
}