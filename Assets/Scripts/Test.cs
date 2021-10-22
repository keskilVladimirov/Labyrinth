using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject playerPref;
    private Player player;
    
    private void Start()
    {
        var playerPos = new Vector3(5,5, 6);
        var playerObject = Instantiate(playerPref, playerPos, Quaternion.identity);
        player = playerObject.GetComponent<Player>();
            
        MouseLeftClicks().Subscribe(p =>
        {
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(p);
            player.SetTargetPosition(mouseWorldPosition);
            
            var mouseTuple = grid.GetXY(mouseWorldPosition);
            var playerTuple = grid.GetXY(player.gameObject.transform.position);
            
            List<Node> path = pathfinding.FindPath(playerTuple.Item1, playerTuple.Item2, mouseTuple.Item1, mouseTuple.Item2);
            if (path != null) 
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].X, path[i].Y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].X, path[i + 1].Y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }
        });
        
        
        MouseRightClicks().Subscribe(p =>
        {
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(p);
            
            var mouseTuple = grid.GetXY(mouseWorldPosition);
            var node = pathfinding.GetNode(mouseTuple.Item1, mouseTuple.Item2);
            if(node != null)
                node.SetIsWalkable(!pathfinding.GetNode(mouseTuple.Item1, mouseTuple.Item2).IsWalkable);
        });
    }

    public void ResetPlayerPosition()
    {
        if(player != null)
            player.transform.position = new Vector3(5,5, 6);
    }
    
    private IObservable<Vector3> MouseLeftClicks() 
    {
        return Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Input.mousePosition); 
    }
    
    private IObservable<Vector3> MouseRightClicks() 
    {
        return Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Select(_ => Input.mousePosition); 
    }
}
