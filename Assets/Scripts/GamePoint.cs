using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GamePoint", menuName = "Engine/GamePoint")]
public class GamePoint : ScriptableObject {

    private Game game;
    public Game Game { get { return game; } set { game = value; } }

}
