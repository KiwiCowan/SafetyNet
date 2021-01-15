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
    Rules rules;
    Graph boardGraph;
    Board board;
    AgentAI agentAI;

    [SerializeField]
    List<Tile> currentTiles;

    public TextMeshProUGUI currentStateText, currentPlayerText;
    public Button endTurn;
    public bool playerChoose = false;
    public bool AgentThink = false;

    void Awake()
    {        
        instance = this;

        board = BoardGo.GetComponent<Board>();
        agentAI = AgentGo.GetComponent<AgentAI>();
        rules = new Rules();
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
                        ChangeTurn();
                    }

                    break;
                }
            case GameState.AGENTTURN:
                {
                    currentPlayerText.text = "AI";
                    if(!AgentThink)
                    {
                        AgentTurn();
                    }
                    
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

        rules.SetBoard(currentTiles);

        currentState = GameState.PLAYERTURN;
    }

    void InitializeBoard()
    {
        board.BuildDefaultBoard();

        //boardStateGraph = board.GetBoardGraph();
        //Debug.Log(Graph.ToString(boardStateGraph));

        foreach (Tile tile in BoardGo.GetComponentsInChildren<Tile>())
        {
            currentTiles.Add(tile);
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
        AgentThink = true;
        agentAI.AgentTurn(rules);        
    }

    public void ChangeTurn()
    {
        rules.UpdateActions(currentTiles);

        if (currentState == GameState.PLAYERTURN)
        {
            currentState = GameState.AGENTTURN;
        }
        else if (currentState == GameState.AGENTTURN)
        {
            currentState = GameState.PLAYERTURN;
        }
    }

    public bool IsGameOver()
    {

        return false;
    }

}

public enum GameState
{
    SETUP,
    PLAYERTURN,
    AGENTTURN,
    ENDGAME
}
