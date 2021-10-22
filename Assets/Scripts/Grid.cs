using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    
    [SerializeField] private GameObject nodePref;
    private float cellSize;
    private int width;
    private int height;
    private Vector3 originPosition;
    private Node[,] gridArray;
    public int GetWidth => width;
    public int GetHeight => height;
    public float GetCellSize => cellSize;
    
    public int MaxSize {
        get {
            return width * height;
        }
    }

    
    public void GenerateGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        
        gridArray = new Node[width, height];
        
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                var nodeObject = Instantiate(nodePref, transform);
                var pos = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f;
                nodeObject.transform.position = new Vector3(pos.x, pos.y, 10);
                var crurentNode = nodeObject.GetComponent<Node>();
                var index = Random.Range(0, 3);
                bool isWalkable;
                if (index == 1)
                    isWalkable = false;
                else
                    isWalkable = true;
                crurentNode.SetNode(this, x, y, isWalkable);
                gridArray[x, y] = crurentNode;
            }
        }
    }
    
    public List<Node> GetNeighbours(Node node)
    {
        var neighbours = new List<Node>();
        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                var checkX = node.X + x;
                var checkY = node.Y + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(gridArray[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    
    public (int, int) GetXY(Vector3 worldPosition) {
        var x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        var y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return (x, y);
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
    }

    private void SetGridObject(int x, int y, Node value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs {x = x, y = y});
        }
    }

    public void SetGridObject (Vector3 worldPosition, Node value)
    {
        var tuple = GetXY(worldPosition);
        SetGridObject(tuple.Item1, tuple.Item2, value);
    }

    public Node GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return null;
        }
    }
    
    public Node GetGridObject(Vector3 worldPosition)
    {
        var tuple = GetXY(worldPosition);
        return GetGridObject(tuple.Item1, tuple.Item2);
    }
}
