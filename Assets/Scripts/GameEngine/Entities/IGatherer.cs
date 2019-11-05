using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGatherer {

    void Gather(Entity target, MoveFormation formation = null);

}
