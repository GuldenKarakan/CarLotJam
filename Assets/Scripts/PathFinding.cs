using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    PathRequestManager requestManager; // Yol isteklerini y�neten s�n�f
    Grids grid; // Yol gridini temsil eden s�n�f

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>(); // Yol istek y�neticisini al
        grid = GetComponent<Grids>(); // Yol gridini al
    }
    public void StartFinfPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos)); // Yol hesaplamas�n� ba�latan metot
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0]; // Yol �zerindeki waypointler
        bool pathSuccess = false; // Yol hesaplama ba�ar�l� m�?

        Node startNode = grid.NodeFromWorldPoint(startPos); // Ba�lang�� d���m�n� al
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // Hedef d���m�n� al

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // A��k k�meyi temsil eden Heap veri yap�s�
            HashSet<Node> closedSet = new HashSet<Node>(); // Kapal� k�meyi temsil eden Hash Set

            openSet.Add(startNode); // Ba�lang�� d���m�n� a��k k�meye ekle

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // A��k k�meyi en d���k maliyetli d���m� alarak g�ncelle
                closedSet.Add(currentNode); // Se�ilen d���m� kapal� k�me olarak i�aretle

                if (currentNode == targetNode) // Hedef d���me ula��ld� m�?
                {
                    pathSuccess = true; // Yol hesaplama ba�ar�l�
                    break; // D�ng�y� sonland�r
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) // Her kom�u d���m i�in
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) // Ge�ersiz veya kapal� d���mse atla
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); // Yeni maliyeti hesapla

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) // E�er yeni maliyet daha d���kse veya kom�u a��k k�mede de�ilse
                    {
                        neighbour.gCost = newMovementCostToNeighbour; // Yeni maliyeti g�ncelle
                        neighbour.hCost = GetDistance(neighbour, targetNode); // Hedef maliyeti g�ncelle
                        neighbour.parent = currentNode; // Kom�u d���m�n ebeveynini g�ncelle

                        if (!openSet.Contains(neighbour)) // E�er kom�u a��k k�mede de�ilse
                            openSet.Add(neighbour); // Kom�uyu a��k k�meye ekle
                    }

                }
            }
        }
        yield return null; // �terasyon sonunda bir frame beklemek i�in
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); // Yolun waypointlerini olu�tur
        }

        requestManager.FinishProcessingPath(waypoints, pathSuccess); // Yol hesaplama sonucunu yol istek y�neticisine bildir
    }

    // Yolu geriye d�nerek olu�turan metot
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode); // D���m� yola ekle
            currentNode = currentNode.parent; // Bir �nceki d���me ge�
        }
        Vector3[] waypoints = SimplifyPath(path); // Yolu basitle�tir
        Array.Reverse(waypoints); // Yolu ters �evir
        return waypoints; // Waypointleri d�nd�r
    }

    // Yolu basitle�tiren metot
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i].gridX - path[i - 1].gridX, path[i].gridY - path[i - 1].gridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition); // Yeni bir y�nde ise waypointi ekle
            }

            directionOld = directionNew; // Y�n� g�ncelle
        }

        return waypoints.ToArray(); // Waypointleri dizi olarak d�nd�r
    }

    // �ki d���m aras�ndaki mesafeyi hesaplayan metot
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
