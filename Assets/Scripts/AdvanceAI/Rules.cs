using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{

    //Dictionary<GameState, int> states = new Dictionary<GameState, int>() {
    //    { GameState.NONE, 0 },
    //    { GameState.P_WON, 1 },
    //    { GameState.A_WON, 2 },
    //    { GameState.DRAW, 3 }
    //};

    Dictionary<Tile, int> actions = new Dictionary<Tile, int>() {
        {'U', 0},
        {'L', 1},
        {'D', 2},
        {'R', 3}
    };

    Dictionary<RewardState, float> rewards = new Dictionary<RewardState, float>() {
        { RewardState.NONE, 0f },
        { RewardState.LOSS, -10f },
        { RewardState.DRAW, 5f },
        { RewardState.WIN, 10f }
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum RewardState
{
    NONE,
    LOSS,
    DRAW,
    WIN
}

