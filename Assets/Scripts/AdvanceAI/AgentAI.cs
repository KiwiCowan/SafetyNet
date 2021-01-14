using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAI : MonoBehaviour
{
    
    public string solution = "";
    public int epochs = 50;

    [Range(0, 1)]
    public float learningRate = 0.5f;

    [Range(0, 1)]
    public float discountFactor = 0.5f;

    int solutionIndex = 0;
    float startY;

    Board board;
    Graph boardGraph;

    //Dictionary<GameState, int> states = new Dictionary<GameState, int>() {
    //    { GameState.NONE, 0 },
    //    { GameState.P_WON, 1 },
    //    { GameState.A_WON, 2 },
    //    { GameState.DRAW, 3 }
    //};

    Dictionary<char, int> actions = new Dictionary<char, int>() {
        {'U', 0},
        {'L', 1},
        {'D', 2},
        {'R', 3}
    };

    Dictionary<TileType, float> rewards = new Dictionary<TileType, float>() {
        { TileType.EMPTY, 1f },
        { TileType.PSTART, -5f },
        { TileType.ASTART, 0f },
        { TileType.PLAYER, -2f },
        { TileType.AGENT, -3f },
        { TileType.END, 10 },
    };

    public AgentAI(Board _board)
    {
        this.board = _board;
    }

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    public void FindPath()
    {
        boardGraph = board.GetBoardGraph();
        Debug.Log(Graph.ToString(boardGraph));

        QLearning qLearning = new QLearning(epochs, actions, rewards, boardGraph, learningRate, discountFactor);
        qLearning.Train();

        solution = qLearning.GetPath();
        StartMove();
    }

    void StartMove()
    {
        StopAllCoroutines();

        Vector3 startPos = board.start.position;
        startPos.y = startY;

        transform.position = startPos;
        solutionIndex = 0;
        CheckNextMove();
    }

    void CheckNextMove()
    {
        if (solutionIndex < solution.Length)
        {
            StartCoroutine(MoveCoroutine(solution[solutionIndex]));
        }
    }

    IEnumerator MoveCoroutine(char dir, float duration = 0.25f)
    {
        float t = 0;
        Vector3 targetPos;
        Vector3 startPos = transform.position;

        if (dir == 'U')
        {
            targetPos = startPos + Vector3.forward;
        }
        else if (dir == 'R')
        {
            targetPos = startPos + Vector3.right;
        }
        else if (dir == 'D')
        {
            targetPos = startPos + Vector3.back;
        }
        else if (dir == 'L')
        {
            targetPos = startPos + Vector3.left;
        }
        else
        {
            yield break;
        }

        while (t < duration)
        {
            t += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, targetPos, t / duration);
            //Debug.Log(transform.position);

            yield return null;
        }

        transform.position = targetPos;

        solutionIndex++;
        CheckNextMove();
    }
}
