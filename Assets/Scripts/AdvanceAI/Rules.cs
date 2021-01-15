using System.Collections;
using System.Collections.Generic;

public class Rules
{
    //Dictionary<GameState, int> states = new Dictionary<GameState, int>() {
    //    { GameState.NONE, 0 },
    //    { GameState.P_WON, 1 },
    //    { GameState.A_WON, 2 },
    //    { GameState.DRAW, 3 }
    //};
    public List<Tile> board;
    public Dictionary<Coord, int> actions = new Dictionary<Coord, int>() { };

    public Dictionary<RewardState, float> rewards = new Dictionary<RewardState, float>() {
        { RewardState.NONE, 0f },
        { RewardState.LOSS, -10f },
        { RewardState.DRAW, 5f },
        { RewardState.WIN, 10f }
    };

    public Rules()
    {

    }

    public void SetBoard(List<Tile> currentTiles)
    {
        board = currentTiles;
    }

    public void UpdateActions(List<Tile> currentTiles)
    {
        actions.Clear();
        int actionIndex = 0;
        foreach (Tile tile in currentTiles)
        {
            if (tile.type == TileType.EMPTY)
            {
                Coord newCoord = new Coord(tile.row, tile.col);
                actions.Add(newCoord, actionIndex);
                actionIndex++;
            }
        }
    }

    

}
public enum RewardState
{
    NONE,
    LOSS,
    DRAW,
    WIN
}

