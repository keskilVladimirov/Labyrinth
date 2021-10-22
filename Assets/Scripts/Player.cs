using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float speed = 40f;
    private Pathfinding pathfinding;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    
    private void Awake()
    {
        var gridHolder = GameObject.Find("GridHolder");
        pathfinding = gridHolder.GetComponent<Pathfinding>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement() 
    {
        if (pathVectorList != null) 
        {
            var targetPosition = pathVectorList[currentPathIndex];
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                var moveDir = (targetPosition - transform.position).normalized;
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else 
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                }
            }
        }
    }
    
    private void StopMoving() 
    {
        pathVectorList = null;
    }

    private Vector3 GetPosition()
    {
        if (gameObject.activeSelf)
            return transform.position;
        else
            return Vector3.zero;
    }

    public void SetTargetPosition(Vector3 targetPosition) 
    {
        currentPathIndex = 0;
        pathVectorList = pathfinding.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1) 
        {
            pathVectorList.RemoveAt(0);
        }
    }
}
