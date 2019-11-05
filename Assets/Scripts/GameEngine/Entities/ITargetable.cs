using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable {

    ICommand Resolve(List<Entity> e, Vector3 hitPosition);

}
