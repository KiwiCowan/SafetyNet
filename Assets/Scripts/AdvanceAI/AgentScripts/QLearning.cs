using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QLearning 
{
    const float EPSILON = 0.000001f;

    Dictionary<Coord, int> actions;
    Graph states;
    Node currentState;

    float[,] qMatrix;
    float learningRate;
    float discountFactor;
    int epochs = 100;

    Dictionary<Tile, RewardState> rewards;

    public QLearning 
        (
        int epochs,
        Dictionary<Coord, int> actions,
        Dictionary<Tile, RewardState> rewards,
        Graph states,
        float learningRate = 0.1f,
        float discountFactor = 0.5f
        )
    {
        this.states = states;
        this.actions = actions;
        this.rewards = rewards;
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        this.epochs = epochs;

        qMatrix = new float[states.Nodes.Count, actions.Count];
        currentState = states.StartNode;
    }

    public void Train()
    {
        // ----------------------------------------------------
        // Train your agent using the Q-Learning algorithm here
        // ----------------------------------------------------

        for (int i = 0; i < epochs; i++)    //Step 2
        {
            currentState = states.StartNode;
            bool isEndState = false;

            while (!isEndState)
            {
                Node nextState = GetRandomNextState(currentState);  //Step 3

                float maxQ = float.MinValue;

                foreach (Node nextStateNeighbourState in nextState.Neighbours)
                {
                    float qValue = GetQuality(nextState, nextStateNeighbourState);

                    if (qValue > maxQ)
                    {
                        maxQ = qValue;
                    }
                }

                float oldQ = GetQuality(currentState, nextState);

                Tile nextStateReward = GetReward(nextState);
                float newQ = ((1 - learningRate) * oldQ) + (learningRate * ((float)rewards[nextStateReward] + (discountFactor * maxQ)));

                SetQuality(currentState, nextState, newQ);
                currentState = nextState;
                //LogQMatrix(i);

                // if reachs a endState
                if (rewards[currentState.Tile] != RewardState.NONE)
                {
                    isEndState = true;
                }
                //if (currentState.Tile == TileType.END)
                //{
                //    isEndState = true;
                //}
            }
            LogQMatrix(i);
        }
    }

    public string GetPath()
    {
        string path = "";

        Node state = states.StartNode;
        int stateIndex = states.GetNodeIndex(state);
        bool isEnd = false;

        while (!isEnd)
        {
            float qValue = float.MinValue;
            int actionIndex = -1;

            for (int a = 0; a < qMatrix.GetLength(1); a++)
            {
                bool qIsZero = Mathf.Abs(qMatrix[stateIndex, a]) < EPSILON;
                if (qMatrix[stateIndex, a] > qValue && !qIsZero)
                {
                    qValue = qMatrix[stateIndex, a];
                    actionIndex = a;
                }
            }

            Coord action = actions.FirstOrDefault(x => x.Value == actionIndex).Key;
            path += action;

            Node newState = GetStateFromAction(state, action);
            stateIndex = states.GetNodeIndex(newState);
            state = newState;

            Debug.Log(path);
            Tile nextStateReward = GetReward(state);
            if (rewards[currentState.Tile] != RewardState.NONE)
            {
                isEnd = true;
            }
            //isEnd = state.TileType == TileType.END;
        }

        return path;
    }

    void LogQMatrix(int epoch)
    {
        string s = "Epoch " + epoch + " - Q Matrix: \n +---------------------------------+ \n";
        s += string.Format("{0, 8}", "");

        foreach (KeyValuePair<Coord, int> pair in actions)
        {
            s += string.Format("{0, 15}", pair.Key);
        }
        s += '\n';

        for (int r = 0; r < qMatrix.GetLength(0); r++)
        {
            s += string.Format("{0, -8}", Coord.ToString(states.GetNodeByIndex(r).Coord) + ")");
            for (int c = 0; c < qMatrix.GetLength(1); c++)
            {
                s += string.Format("{0, 15}", qMatrix[r, c].ToString("F8"));
            }
            s += '\n';
        }

        Debug.Log(s);
    }

    Node GetRandomNextState(Node state)
    {
        return state.Neighbours[Random.Range(0, currentState.Neighbours.Count)];
    }

    float GetQuality(Node state, Node nextState)
    {
        int actionIndex = actions[GetAction(state, nextState)];
        int stateIndex = states.GetNodeIndex(state);

        return qMatrix[stateIndex, actionIndex];
    }

    void SetQuality(Node state, Node nextState, float value)
    {
        int actionIndex = actions[GetAction(state, nextState)];
        int stateIndex = states.GetNodeIndex(state);

        qMatrix[stateIndex, actionIndex] = value;
    }

    Tile GetReward(Node state)
    {
        Tile rewardTile = new Tile();

        rewardTile = state.Tile;

        return rewardTile;
    }

    Coord GetAction(Node state, Node toState)
    {
        Coord tileCoord = toState.Coord;
        return tileCoord;

        //Coord direction = new Coord(
        //    toState.Coord.row - fromState.Coord.row,
        //    toState.Coord.col - fromState.Coord.col
        //);

        //if (direction.row == 1 && direction.col == 0)
        //{
        //    return 'U';
        //}
        //if (direction.row == -1 && direction.col == 0)
        //{
        //    return 'D';
        //}
        //if (direction.row == 0 && direction.col == 1)
        //{
        //    return 'R';
        //}

        //return 'L';
    }

    Node GetStateFromAction(Node state, Coord action)
    {
        foreach (Node neighbour in state.Neighbours)
        {
            if (neighbour.Coord == action)
            {
                return neighbour;
            }
        }
        return null;

            //Coord dir = ActionToDirection(action);
            //foreach (Node neighbour in state.Neighbours)
            //{
            //    int r = neighbour.Coord.row - state.Coord.row;
            //    int c = neighbour.Coord.col - state.Coord.col;

            //    if (dir.col == c && dir.row == r)
            //    {
            //        return neighbour;
            //    }
            //}
            //return null;
    }

    //Coord ActionToDirection(char action)
    //{
    //    switch (action)
    //    {
    //        case 'U': return new Coord(1, 0);
    //        case 'R': return new Coord(0, 1);
    //        case 'D': return new Coord(-1, 0);
    //        default: return new Coord(0, -1);
    //    }
    //}
}
