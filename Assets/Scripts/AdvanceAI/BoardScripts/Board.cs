using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Range(3, 10)]
    public int rows = 5;
    [Range(3, 10)]
    public int columns = 5;

    public GameObject tileTemplate;
    private Tile[,] tiles = new Tile[0, 0];

    public Transform playerStart;
    public Transform start;         //AI start 
    public Transform end;

    readonly Coord[] coordNeighbours = { new Coord(1, 0), new Coord(0, 1), new Coord(-1, 0), new Coord(0, -1) };
    Graph graph;


    public int[,] defaultBoard = new int[,]
    {
        // 5x5 board
        { 0, 0, 1, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 5, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0 },
        { 0, 0, 2, 0, 0 }
        

        // test maze 6x6
        //{ 2, 0, 0, 0, 0, 3 },
        //{ 3, 0, 3, 3, 0, 3 },
        //{ 3, 0, 3, 3, 0, 3 },
        //{ 3, 0, 3, 3, 0, 1 },
        //{ 0, 0, 0, 0, 0, 0 },
        //{ 0, 3, 3, 3, 3, 5 },
    };

    // Start is called before the first frame update
    void Start()
    {

        //BuildMaze();
        //BuildDefaultBoard();
        
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
        start = startTile.transform;
    }

    private void OnTileEndSet(Tile endTile)
    {
        end = endTile.transform;
    }

    public Graph GetBoardGraph()
    {

        //find start coordinate
        Coord startCoord = GetStartCoord();

        if (startCoord == null)
            return null;
               
        //create start node and initialize graph
        Node startNode = new Node(tiles[startCoord.row, startCoord.col], startCoord);
        graph = new Graph(startNode);

        List<Coord> keys = new List<Coord>();
        keys.Add(startNode.Coord);

        //create all other nodes
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Tile tile = tiles[r, c];
                //TileType tileType = tiles[r, c].type;
                if (tile != startNode.Tile/*tileType != TileType.ASTART*/ /*&& tileType != TileType.PSTART*/)
                {
                    Coord nodeCoord = new Coord(r, c);
                    graph.AddNode(tile, nodeCoord);
                    keys.Add(nodeCoord);
                }
            }
        }

        //connect all nodes
        foreach (Coord key in keys)
        {
            Node currentNode = graph.FindNodeByCoord(key);
            if (currentNode != null)
            {

                ConnectNeighbours(currentNode);
            }
        }

        return graph;
    }

    private Coord GetStartCoord()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (tiles[r, c].type == TileType.ASTART)
                {
                    return new Coord(r, c);
                }
            }
        }

        return null;
    }

    private void ConnectNeighbours(Node node)
    {
        
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Node neighbourNode = graph.FindNodeByCoord(new Coord(r, c));

                if (neighbourNode == null)
                    continue;

                if ( neighbourNode != node && neighbourNode.Tile.type == TileType.EMPTY)
                {                   
                    node.AddNeighbour(neighbourNode);
                }
            }
        }

        ////return null;

        //foreach (Coord neighbour in coordNeighbours)
        //{
        //    int row = node.Coord.row + neighbour.row;
        //    int col = node.Coord.col + neighbour.col;

        //    Node neighbourNode = graph.FindNodeByCoord(new Coord(row, col));
        //    if (neighbourNode == null)
        //        continue;

        //    node.AddNeighbour(neighbourNode);
        //}
    }

    public void BuildDefaultBoard()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject)
                Destroy(child.gameObject);
        }

        tiles = new Tile[rows, columns];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                //int tileIndex = defaultBoard[r, c];
                GameObject tileGameObject = Instantiate(tileTemplate);
                tileGameObject.transform.parent = transform;
                tileGameObject.transform.position = new Vector3(-columns / 2.0f + c + 0.5f, 0, -rows / 2.0f + r + 0.5f);
                tileGameObject.SetActive(true);
                tileGameObject.name = ("Tile_" + r + "_" + c + "_" + (TileType)defaultBoard[r, c]);
                tileGameObject.transform.GetChild(defaultBoard[r, c]).gameObject.SetActive(true);
                //tileGameObject.GetComponentInChildren

                Tile tile = tileGameObject.GetComponent<Tile>();
                tile.row = r;
                tile.col = c;
                tile.type = (TileType)defaultBoard[r, c];
                
                tiles[r, c] = tile;

                if (tile.type == TileType.ASTART)
                {
                    start = tile.transform;
                }

                if (tile.type == TileType.PSTART)
                {
                    playerStart = tile.transform;
                }

                if (tile.type == TileType.END)
                {
                    end = tile.transform;
                }
                //tile.UpdateTileType();
                //tiles[r, c].SetType((TileType)defaultBoard[r, c]);
            }
        }

        //foreach (Tile tile in tiles)
        //{
        //    //Debug.Log("Tile at: " + tile.row + " , " + tile.col + " is TileType: " + tile.type);
        //    tile.UpdateTileType();
        //    Debug.Log("Tile at: " + tile.row + " , " + tile.col + " is TileType: " + tile.type);
        //}
    }

    //private void BuildMaze()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        if (child.gameObject)
    //            Destroy(child.gameObject);
    //    }

    //    tiles = new Tile[rows, columns];

    //    for (int r = 0; r < rows; r++)
    //    {
    //        for (int c = 0; c < columns; c++)
    //        {
    //            GameObject tileGameObject = Instantiate(tileTemplate);
    //            tileGameObject.transform.parent = transform;
    //            tileGameObject.transform.position = new Vector3(-columns / 2.0f + c + 0.5f, 0, -rows / 2.0f + r + 0.5f);
    //            tileGameObject.SetActive(true);

    //            Tile tile = tileGameObject.GetComponent<Tile>();
    //            tile.row = r;
    //            tile.col = c;
    //            tiles[r, c] = tile;
    //        }
    //    }
    //}
}


