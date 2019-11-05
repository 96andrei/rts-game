using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ui Data", menuName = "Engine/Entities/New Ui Data")]
public class EntityUiData : ScriptableObject {

    [SerializeField]
    Sprite sprite;

    [SerializeField]
    string displayName;

    public Sprite Sprite { get { return sprite; } }
    public string DisplayName { get { return displayName; } }
}
