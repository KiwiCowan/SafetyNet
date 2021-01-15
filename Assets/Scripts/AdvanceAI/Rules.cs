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
        bool isAgent = true;
        
        if (IsLastMove(boardList))   // Checks if its the last move
        {
            foreach (Tile tile in boardList)
            {
                if (tile.type == TileType.EMPTY)
                {
                    if (isAgent) // if its the Agent's turn last
                    {
                        // Check for Agent Win
                        if (IsRowFull(tile, TileType.AGENT) || IsColFull(tile, TileType.AGENT) || IsDiaFull(tile, TileType.AGENT))
                        {
                            rewardTiles.Add(tile, (float)RewardState.WIN);
                        }
                        else    // if no win, it can only be a Draw
                        {
                            rewardTiles.Add(tile, (float)RewardState.DRAW);
                        }
                    }
                    else    // if its the Player's turn last
                    {
                        // Check for Player Win / Agent Loss
                        if (IsRowFull(tile, TileType.PLAYER) || IsColFull(tile, TileType.PLAYER) || IsDiaFull(tile, TileType.PLAYER))
                        {
                            rewardTiles.Add(tile, (float)RewardState.LOSS);
                        }
                        else
                        {
                            rewardTiles.Add(tile, (float)RewardState.DRAW);
                        }
                    }                   
                }
            }
        }
        else    // if there is more than 1 moves left 
        {
            foreach (Tile tile in boardList)
            {
                if (tile.type == TileType.EMPTY)
                {
                    if (isAgent) // if its the Agent's turn
                    {
                        // Check for Agent Win
                        if (IsRowFull(tile, TileType.AGENT) || IsColFull(tile, TileType.AGENT) || IsDiaFull(tile, TileType.AGENT))
                        {
                            rewardTiles.Add(tile, (float)RewardState.WIN);
                        }
                        else    // if no win, it can only be a None
                        {
                            rewardTiles.Add(tile, (float)RewardState.NONE);
                        }
                    }
                    else    // if its the Player's turn
                    {
                        // Check for Player Win / Agent Loss
                        if (IsRowFull(tile, TileType.PLAYER) || IsColFull(tile, TileType.PLAYER) || IsDiaFull(tile, TileType.PLAYER))
                        {
                            rewardTiles.Add(tile, (float)RewardState.LOSS);
                        }
                        else
                        {
                            rewardTiles.Add(tile, (float)RewardState.NONE);
                        }
                    }
                }
            }
        }

        //if (IsLastMove(boardList))   // Checks if its the last move
        //{
        //    foreach (Tile tile in boardList)
        //    {
        //        if (tile.type == TileType.EMPTY)
        //        {
        //            // Check for Agent Win
        //            if (IsRowFull(tile, TileType.AGENT) || IsColFull(tile, TileType.AGENT) || IsDiaFull(tile, TileType.AGENT))
        //            {
        //                rewardTiles.Add(tile, (float)RewardState.WIN);
        //            } // Check for Agent Loss by checking if the player will win in the next move
        //            else if (IsRowFull(tile, TileType.PLAYER) || IsColFull(tile, TileType.PLAYER) || IsDiaFull(tile, TileType.PLAYER))
        //            {
        //                rewardTiles.Add(tile, (float)RewardState.LOSS);
        //            }
        //            else
        //            {
        //                rewardTiles.Add(tile, (float)RewardState.DRAW);
        //            }
        //        }
        //    }
        //}

        //int full = boardList.Count;
        //Tile lastEmptyTile = null;
        //foreach (Tile tile in boardList)
        //{
        //    if (tile.type == TileType.EMPTY)
        //    {
        //        if (lastEmptyTile == null)
        //        {
        //            lastEmptyTile = tile;
        //        }
        //        // Check for Agent Win
        //        if (IsRowFull(tile, TileType.AGENT) || IsColFull(tile, TileType.AGENT) || IsDiaFull(tile, TileType.AGENT))
        //        {
        //            rewardTiles.Add(tile, (float)RewardState.WIN);
        //        } // Check for Agent Loss by checking if the player will win in the next move
        //        else if (IsRowFull(tile, TileType.PLAYER) || IsColFull(tile, TileType.PLAYER) || IsDiaFull(tile, TileType.PLAYER))
        //        {
        //            rewardTiles.Add(tile, (float)RewardState.LOSS);
        //        }





        //    }
        //    else
        //    {
        //        full--;
        //    }
        //}

        //// Check for Agent Draw by checking if the are no empty tiles
        //if (full == 1)
        //{

        //}
        //else if (full == boardList.Count)   // Check for if no 
        //{

        //}
    }

    public bool IsRowFull(Tile tempMove, TileType currentPlayer)
    {

        int r = tempMove.row;
        List<TileType> tileRow = new List<TileType>();

        for (int c = 0; c < cols; c++)
        {
            TileType tile;

            if (c == tempMove.col)
            {
                tile = currentPlayer;
            }
            else
            {
                tile = board.tiles[r, c].type;
            }

            tileRow.Add(tile);
        }

        return IsVictory(cols, tileRow, currentPlayer);
    }

    public bool IsColFull(Tile tempMove, TileType currentPlayer)
    {
        int c = tempMove.col;
        List<TileType> tileCol = new List<TileType>();

        for (int r = 0; r < rows; r++)
        {
            TileType tile;

            if (r == tempMove.row)
            {
                tile = currentPlayer;
            }
            else
            {
                tile = board.tiles[r, c].type;
            }

            tileCol.Add(tile);
        }

        return IsVictory(rows, tileCol, currentPlayer);
    }

    public bool IsDiaFull(Tile tempMove, TileType currentPlayer)
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
                            tile = currentPlayer;
                        }
                        else
                        {
                            tile = board.tiles[r, c].type;
                        }

                        tileDia1.Add(tile);
                    }
                }
            }
            return IsVictory((rows + cols) / 2, tileDia1, currentPlayer);
        }

        List<TileType> tileDia2 = new List<TileType>();

        int col = cols - 1;

        if (col >= 0)
        {
            for (int r = 0; r < rows; r++)
            {
                TileType tile;

                if (r == tempMove.row && col == tempMove.col)
                {
                    tile = currentPlayer;
                }
                else
                {
                    tile = board.tiles[r, col].type;
                }

                tileDia2.Add(tile);
                col--;

            }

            return IsVictory((rows + cols) / 2, tileDia2, currentPlayer);
        }

        return false;
    }



    public bool IsVictory(int _full, List<TileType> tileTypes, TileType currentPlayer)
    {
        int full = _full;
        foreach (TileType tileType in tileTypes)
        {
            if (tileType == currentPlayer)
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

    public bool IsLastMove(List<Tile> fullBoardList)
    {
        int movesLeft = 0;
        foreach (Tile tile in fullBoardList)
        {
            if (tile.type == TileType.EMPTY)
            {
                movesLeft++;
            }

        }

        if (movesLeft > 1)
        {
            return false;
        }
        else if (movesLeft == 1)
        {
            return true;
        }

        return false;
    }

}

public enum RewardState
{
    NONE = 0,
    LOSS = -10,
    DRAW = 5,
    WIN = 10
}

