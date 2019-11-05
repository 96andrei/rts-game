using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Entity Res Generator Stats", menuName = "Engine/Entities/New Entity Res Generator Stats")]
public class EntityGeneratorStats : ScriptableObject {

    [SerializeField]
    private int minValue = 10;
    [SerializeField]
    private int maxValue = 15;
    [SerializeField]
    private float interval = 5;

    public int Value { get { return Random.Range(minValue, maxValue); } }
    public float Interval { get { return interval; } }
    public int MinValue { get { return minValue; } }
    public int MaxValue { get { return maxValue; } }

}
