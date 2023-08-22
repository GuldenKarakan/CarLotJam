using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    [HideInInspector] public bool pathSuccess = false;
    PathRequestManager requestManager; 
    Grids grid; 

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>(); 
        grid = GetComponent<Grids>();
    }
    public void StartFinfPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos)); 
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        grid.ResetGrid();

        Vector3[] waypoints = new Vector3[0];  
        pathSuccess = false;  

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);  

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break; 
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour; 
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode; 
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }

                }
            }
        }
        yield return null; 
        if (pathSuccess)
            waypoints = RetracePath(startNode, targetNode); 
        requestManager.FinishProcessingPath(waypoints, pathSuccess);
    }

    // Yolu geriye dönerek oluþturan metot
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode); 
            currentNode = currentNode.parent;
            
        }
        path.Add(startNode);
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    // Yolu basitleþtiren metot
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            waypoints.Add(path[i].worldPosition);
        }

        return waypoints.ToArray();
    }

    // Ýki düðüm arasýndaki mesafeyi hesaplayan metot
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 20 * dstY + 10 * (dstX - dstY);
        return 20 * dstX + 10 * (dstY - dstX);
    }
}

