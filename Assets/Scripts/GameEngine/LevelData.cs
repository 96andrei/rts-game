using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Engine/Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private string levelName;
    public string Name { get { return levelName; } }
}
