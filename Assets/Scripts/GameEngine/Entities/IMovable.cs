using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IMovable  {

    NavMeshAgent Agent { get; }
    MoveFormation Formation { get; }
    bool WorkingFormation();
    bool Suspended { get; set; }
    float Speed { get; }
    void Move(Vector3 destination, MoveFormation formation);
    void Stop();
}
