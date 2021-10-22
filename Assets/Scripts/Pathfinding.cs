using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Pathfinding : MonoBehaviour
{
   private const int MOVE_STRAIGHT_COST = 10;
   private const int MOVE_DIAGONAL_COST = 14;
   private Grid grid;
   private Heap<Node> openList;
   private List<Node> closedList;

   private void Awake()
   {
      grid = GetComponent<Grid>();
   }

   public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) 
   {
      var startTuple = grid.GetXY(startWorldPosition);
      var endTuple = grid.GetXY(endWorldPosition);

      var path = FindPath(startTuple.Item1, startTuple.Item2, endTuple.Item1, endTuple.Item2);
      if (path == null) 
      {
         return null;
      } 
      else 
      {
         var vectorPath = new List<Vector3>();
         foreach (var pathNode in path) 
         {
            vectorPath.Add(new Vector3(pathNode.X, pathNode.Y) * grid.GetCellSize + Vector3.one * (grid.GetCellSize * .5f));
         }
         return vectorPath;
      }
   }
   
   public List<Node> FindPath(int startX, int startY, int endX, int endY)
   {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      var startNode = grid.GetGridObject(startX, startY);
      var endNode = grid.GetGridObject(endX, endY);
      
      if (startNode == null || endNode == null || !endNode.IsWalkable) { return null; }
      
      openList = new Heap<Node>(grid.MaxSize);
      closedList = new List<Node>();
      openList.Add(startNode);
      
      for (var x = 0; x < grid.GetWidth; x++) 
      {
         for (var y = 0; y < grid.GetHeight; y++) 
         {
            var node = grid.GetGridObject(x, y);
            node.GCost = 99999999;
            node.CalculateFCost();
            node.Parent = null;
         }
      }
      
      startNode.GCost = 0;
      startNode.HCost = GetDistance(startNode, endNode);
      startNode.CalculateFCost();
      
      
      while (openList.Count > 0)
      {
         var currentNode = openList.RemoveFirst();
         closedList.Add(currentNode);
         
         if (currentNode == endNode)
         {
            sw.Stop();
            print("Путь найден за " + sw.ElapsedMilliseconds + " ms");
            return RetracePath(currentNode);
         }

         foreach (var neighbour in grid.GetNeighbours(currentNode))
         {
            if (closedList.Contains(neighbour)) continue;
            if (!neighbour.IsWalkable)
            {
               closedList.Add(neighbour); 
               continue;
            }
            
            var newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
            if (newMovementCostToNeighbour < neighbour.GCost || !openList.Contains(neighbour))
            {
               neighbour.Parent = currentNode;
               neighbour.GCost = newMovementCostToNeighbour;
               neighbour.HCost = GetDistance(neighbour, endNode);
               neighbour.CalculateFCost();
               
               if(!openList.Contains(neighbour))
                  openList.Add(neighbour);
            }
         }
      }
      return null;
   }

   private List<Node> RetracePath(Node endNode)
   {
      var path = new List<Node>{endNode};
      var currentNode = endNode;
      
      while (currentNode.Parent != null)
      {
         path.Add(currentNode.Parent);
         currentNode = currentNode.Parent;
      }
      path.Reverse();
      return path;
   }

   private int GetDistance(Node nodeA, Node nodeB)
   {
      var xDistance = (int)Mathf.Abs(nodeA.X - nodeB.X);
      var yDistance = (int)Mathf.Abs(nodeA.Y - nodeB.Y);
      return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) +
             MOVE_STRAIGHT_COST * Mathf.Abs(xDistance - yDistance);
   }
   
   private Node GetLowestFCostNode(List<Node> pathNodeList) 
   {
      var lowestFCostNode = pathNodeList[0];
      for (var i = 1; i < pathNodeList.Count; i++) 
      {
         if (pathNodeList[i].FCost < lowestFCostNode.FCost) 
         {
            lowestFCostNode = pathNodeList[i];
         }
      }
      return lowestFCostNode;
   }
   
   public Node GetNode(int x, int y) 
   {
      return grid.GetGridObject(x, y);
   }
}
