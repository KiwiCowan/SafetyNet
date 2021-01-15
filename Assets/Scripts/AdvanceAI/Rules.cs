using System.Collections;
using System.Collections.Generic;

public class Rules
{
    int rows;
    int cols;

    public List<Tile> boardList;
    Board board;
    public Dictionary<Coord, int> actions = new Dictionary<Coord, int>() { };

    public Dictionary<Tile, float> rewardTiles = new Dictionary<Tile, float>() { };

    public Dictionary<RewardState, float> rewards = new Dictionary<RewardState, float>() {
        { RewardState.NONE, 0f },
        { RewardState.LOSS, -10f },
        { RewardState.DRAW, 5f },
        { RewardState.WIN, 10f }
    };

    public Rules(Board _board)
    {
        this.board = _board;
    }

    public void SetBoard(List<Tile> currentTiles)
    {
        boardList = currentTiles;
        rows = board.rows;
        cols = board.columns;
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

    public void GetRewards()
    {
        foreach (Tile tile in boardList)
        {
            if (tile.type == TileType.EMPTY)
            {
                Coord tempMove = new Coord(tile.row, tile.col);
                if (IsRowFull(tile))
                {

                }
            }
        }
    }

    public bool IsRowFull(Tile tempMove)
    {
        int r = tempMove.row;
        List<TileType> tileRow = new List<TileType>();
        
        for (int c = 0; c < cols; c++)
        {
            TileType tile;

            if (c == tempMove.col)
            {
                tile = TileType.AGENT;
            }
            else
            {
                tile = board.tiles[r, c].type;
            }
            
            tileRow.Add(tile);
        }

        return IsVictory(cols,tileRow);
    }

    public bool IsColFull(Tile tempMove)
    {
        int c = tempMove.col;
        List<TileType> tileCol = new List<TileType>();

        for (int r = 0; r < rows; r++)
        {
            TileType tile;

            if (r == tempMove.row)
            {
                tile = TileType.AGENT;
            }
            else
            {
                tile = board.tiles[r, c].type;
            }

            tileCol.Add(tile);
        }

        return IsVictory(rows, tileCol);
    }

    public bool IsDiaFull(Tile tempMove)
    {
        List<TileType> tileDia1 = new List<TileType>();
        if (tempMove.row == tempMove.col)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (r == c)
                    {
                        TileType tile;

                        if (r == tempMove.row && c == tempMove.col)
                        {
                            tile = TileType.AGENT;
                        }
                        else
                        {
                            tile = board.tiles[r, c].type;
                        }

                        tileDia1.Add(tile);
                    }
            }
            }
            return IsVictory((rows + cols)/2, tileDia1);
        }

        List<TileType> tileDia2 = new List<TileType>();
        if (tempMove.row == tempMove.col)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = cols; c < 0; c--)
                {
                    if (r == c)
                    {
                        TileType tile;

                        if (r == tempMove.row && c == tempMove.col)
                        {
                            tile = TileType.AGENT;
                        }
                        else
                        {
                            tile = board.tiles[r, c].type;
                        }

                        tileDia2.Add(tile);
                    }
                }
            }
            return IsVictory((rows + cols) / 2, tileDia2);
        }

        return false;
    }

    public bool IsVictory(int _full, List<TileType> tileTypes)
    {
        int full = _full;
        foreach (TileType tileType in tileTypes)
        {
            if (tileType == TileType.AGENT)
            {
                full--;
            }
        }
        if (full == 0)
        {
            return true;
        }

        return false;
    }

}

public enum RewardState
{
    NONE,
    LOSS,
    DRAW,
    WIN
}

