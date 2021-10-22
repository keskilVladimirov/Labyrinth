using Interface;
using UnityEngine;

public class Node : MonoBehaviour, IHeapItem<Node>
{
    [SerializeField] private GameObject green;
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject blue;
    
    private Grid grid;
    private int x;
    private int y;
    private bool isWalkable;
    private Node parent; 
    private int gCost;
    private int hCost;
    private int fCost;
    private int heapIndex;

    public int X => x;
    public int Y => y;
    public bool IsWalkable => isWalkable;
    public Node Parent
    {
        get => parent;
        set => parent = value;
    }

    public int GCost
    {
        get => gCost;
        set => gCost = value;
    }

    public int HCost
    {
        get => hCost;
        set => hCost = value;
    }
    
    public int HeapIndex
    {
        get => heapIndex;
        set => heapIndex = value;
    }

    public int FCost => fCost;
    
    public void SetNode(Grid grid, int x, int y, bool isWalkable)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        SetIsWalkable(isWalkable);
    }
    
    public void SetIsWalkable(bool isWalkable) 
    {
        this.isWalkable = isWalkable;
        green.SetActive(isWalkable);
        black.SetActive(!isWalkable);
        grid.TriggerGridObjectChanged(x, y);
    }
    
    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public int CompareTo(Node other)
    {
        var compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }
        return -compare;
    }
}
