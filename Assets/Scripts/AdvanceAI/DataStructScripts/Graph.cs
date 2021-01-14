using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph 
{
    public Dictionary<string, Node> Nodes { get; private set; } = new Dictionary<string, Node>();
    Dictionary<string, int> indices = new Dictionary<string, int>();
    public Node StartNode { get; private set; }

    int count = 0;

    public Graph(Node startNode)
    {
        StartNode = startNode;
        string key = Coord.ToString(StartNode.Coord);
        Nodes.Add(key, StartNode);
        AddIndex(key);
    }

    public void AddNode(TileType tileType, Coord coord)
    {
        Node newNode = new Node(tileType, coord);
        string key = Coord.ToString(newNode.Coord);
        Nodes.Add(key, newNode);
        AddIndex(key);
    }

    public void AddNode(Node node)
    {
        string key = Coord.ToString(node.Coord);
        Nodes.Add(key, node);
        AddIndex(key);
    }

    public Node GetNodeByIndex(int index)
    {
        return Nodes.Values.ElementAt(index);
    }

    public int GetNodeIndex(Node node)
    {
        string key = Coord.ToString(node.Coord);
        return indices[key];
    }

    public Node FindNodeByCoord(Coord coord)
    {
        Node outNode;

        if (Nodes.TryGetValue(Coord.ToString(coord), out outNode))
        {
            return outNode;
        }

        return null;
    }

    private void AddIndex(string key)
    {
        indices.Add(key, count);
        count++;
    }

    public static string ToString(Graph instance)
    {
        string s = "graph: " + instance.Nodes.Count + "\n";
        foreach (KeyValuePair<string, Node> pair in instance.Nodes)
        {
            s += pair.Value.Coord.row + "-" + pair.Value.Coord.col + " => ";

            foreach (Node neighbour in pair.Value.Neighbours)
            {
                s += neighbour.Coord.row + "-" + neighbour.Coord.col + ", ";
            }

            s += "\n";
        }

        return s;
    }
}
