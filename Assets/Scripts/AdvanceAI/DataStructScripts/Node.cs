using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> Neighbours { get; private set; } = new List<Node>();
    //public TileType TileType { get; private set; }
    public Coord Coord { get; private set; }

    public Tile Tile { get; private set; }

    public Node(/*TileType tileType*/Tile tile, Coord coord)
    {
        //TileType = tileType;
        Coord = coord;
        Tile = tile;
    }

    public void AddNeighbour(Node node)
    {
        if (this.HasNeighbour(node))
            return;

        Neighbours.Add(node);
    }

    public bool HasNeighbour(Node node)
    {
        foreach (Node neighbour in Neighbours)
        {
            if (neighbour.Coord.row == node.Coord.row && neighbour.Coord.col == node.Coord.col)
            {
                return true;
            }
        }

        return false;
    }
}
