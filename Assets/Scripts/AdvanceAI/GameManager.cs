using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject BoardGo;
    public GameObject PlayerGo;
    public GameObject AgentGo;

    public GameState currentState;
    Graph boardGraph;
    Board board;
    AgentAI agentAI;

    [SerializeField]
    List<Tile> currentTiles;

    public TextMeshProUGUI currentStateText, currentPlayerText;
    public Button endTurn;
    public bool playerChoose = false;

    void Awake()
    {        
        instance = this;

        board = BoardGo.GetComponent<Board>();
        agentAI = AgentGo.GetComponent<AgentAI>();
      
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.SETUP;
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.SETUP:
                {
                    currentPlayerText.text = "Setup";
                    break;
                }
            case GameState.PLAYERTURN:
                {
                    currentPlayerText.text = "Player";
                   
                    if(playerChoose)
                    {
                        currentState = GameState.AGENTTURN;
                    }

                    break;
                }
            case GameState.AGENTTURN:
                {
                    currentPlayerText.text = "AI";
                    AgentTurn();
                    break;
                }
            case GameState.ENDGAME:
                {
                    currentPlayerText.text = "End Game";
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void OnEnable()
    {      
        Tile.OnPlayerChoice += PlayerTurn;
    }

    private void OnDisable()
    {
        Tile.OnPlayerChoice -= PlayerTurn;
    }

    void Setup()
    {
        // Initialize the Board
        InitializeBoard();

        // Spawn units
        PlaceUnits();

        currentState = GameState.PLAYERTURN;
    }

    void InitializeBoard()
    {
        board.BuildDefaultBoard();

        boardGraph = board.GetBoardGraph();
        Debug.Log(Graph.ToString(boardGraph));

        foreach (Tile tileGo in BoardGo.GetComponentsInChildren<Tile>())
        {
            currentTiles.Add(tileGo);
        }
    }

    void PlaceUnits()
    {
        PlayerGo.transform.position = board.playerStart.position;
        AgentGo.transform.position = board.start.position;
    }

    void PlayerTurn(Tile tile)
    {
        Vector3 newPos = tile.gameObject.transform.position;
       
        // When player choose tile move playerGO pos
        PlayerGo.transform.position = newPos;

        playerChoose = true;       
    }

    void AgentTurn()
    {
        playerChoose = false;
    }
}

public enum GameState
{
    SETUP,
    PLAYERTURN,
    AGENTTURN,
    ENDGAME
}
