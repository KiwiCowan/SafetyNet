using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public delegate void StartSet(Tile tile);
    public static event StartSet OnStartSet;

    public delegate void PlayerChoice(Tile tile);
    public static event PlayerChoice OnPlayerChoice;

    public delegate void EndSet(Tile mazeTtileile);
    public static event EndSet OnEndSet;

    public int row;
    public int col;
    public TileType type = TileType.EMPTY;

    public GameObject currentTile;
    GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTile = transform.GetChild((int)type).gameObject;        
    }

    private void OnEnable()
    {
        Tile.OnStartSet += OnTileStartSet;
        Tile.OnEndSet += OnTileEndSet;
    }

    private void OnDisable()
    {
        Tile.OnStartSet -= OnTileStartSet;
        Tile.OnEndSet -= OnTileEndSet;

    }

    private void OnTileStartSet(Tile startTile)
    {
        if (startTile == this || type != TileType.ASTART)
            return;

        SetType(TileType.EMPTY);
    }

    private void OnTileEndSet(Tile endTile)
    {
        if (endTile == this || type != TileType.END)
            return;

        SetType(TileType.EMPTY);
    }

    private void OnMouseDown()
    {
        if (gameManager.currentState == GameState.AGENTTURN)
        {
            return;
        }
        else if (gameManager.currentState == GameState.PLAYERTURN)
        {
            if ( OnPlayerChoice != null)
            {
                OnPlayerChoice(this);
            }
            SetType(TileType.PLAYER);
        }

        //int typeNumber = (int)type + 1;

        //if (typeNumber > 5)
        //    typeNumber = 0;

        //TileType tileType = (TileType)typeNumber;

        //SetType(tileType);
    }

    private void SetType(TileType newTileType)
    {
        int typeNumber = (int)newTileType;

        GameObject newTile = transform.GetChild(typeNumber).gameObject;
        newTile.SetActive(true);
        currentTile.SetActive(false);

        currentTile = newTile;
        type = newTileType;

        if (type == TileType.ASTART && OnStartSet != null)
        {
            OnStartSet(this);
        }

        if (type == TileType.END && OnEndSet != null)
        {
            OnEndSet(this);
        }
    }
}

public enum TileType
{
    EMPTY = 0,
    PSTART = 1,
    ASTART = 2,
    PLAYER = 3,
    AGENT = 4,    
    END = 5
}
