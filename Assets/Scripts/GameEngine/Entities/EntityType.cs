using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Type", menuName = "Engine/Entities/New Type")]
public class EntityType : ScriptableObject {

    [SerializeField]
    private string typeId;

    public string TypeId { get { return typeId; } }

}
