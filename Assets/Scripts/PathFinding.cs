using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    PathRequestManager requestManager; // Yol isteklerini yöneten sýnýf
    Grids grid; // Yol gridini temsil eden sýnýf

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>(); // Yol istek yöneticisini al
        grid = GetComponent<Grids>(); // Yol gridini al
    }
    public void StartFinfPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos)); // Yol hesaplamasýný baþlatan metot
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0]; // Yol üzerindeki waypointler
        bool pathSuccess = false; // Yol hesaplama baþarýlý mý?

        Node startNode = grid.NodeFromWorldPoint(startPos); // Baþlangýç düðümünü al
        Node targetNode = grid.NodeFromWorldPoint(targetPos); // Hedef düðümünü al

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Açýk kümeyi temsil eden Heap veri yapýsý
            HashSet<Node> closedSet = new HashSet<Node>(); // Kapalý kümeyi temsil eden Hash Set

            openSet.Add(startNode); // Baþlangýç düðümünü açýk kümeye ekle

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst(); // Açýk kümeyi en düþük maliyetli düðümü alarak güncelle
                closedSet.Add(currentNode); // Seçilen düðümü kapalý küme olarak iþaretle

                if (currentNode == targetNode) // Hedef düðüme ulaþýldý mý?
                {
                    pathSuccess = true; // Yol hesaplama baþarýlý
                    break; // Döngüyü sonlandýr
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode)) // Her komþu düðüm için
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) // Geçersiz veya kapalý düðümse atla
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); // Yeni maliyeti hesapla

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) // Eðer yeni maliyet daha düþükse veya komþu açýk kümede deðilse
                    {
                        neighbour.gCost = newMovementCostToNeighbour; // Yeni maliyeti güncelle
                        neighbour.hCost = GetDistance(neighbour, targetNode); // Hedef maliyeti güncelle
                        neighbour.parent = currentNode; // Komþu düðümün ebeveynini güncelle

                        if (!openSet.Contains(neighbour)) // Eðer komþu açýk kümede deðilse
                            openSet.Add(neighbour); // Komþuyu açýk kümeye ekle
                    }

                }
            }
        }
        yield return null; // Ýterasyon sonunda bir frame beklemek için
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode); // Yolun waypointlerini oluþtur
        }

        requestManager.FinishProcessingPath(waypoints, pathSuccess); // Yol hesaplama sonucunu yol istek yöneticisine bildir
    }

    // Yolu geriye dönerek oluþturan metot
    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode); // Düðümü yola ekle
            currentNode = currentNode.parent; // Bir önceki düðüme geç
        }
        Vector3[] waypoints = SimplifyPath(path); // Yolu basitleþtir
        Array.Reverse(waypoints); // Yolu ters çevir
        return waypoints; // Waypointleri döndür
    }

    // Yolu basitleþtiren metot
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i].gridX - path[i - 1].gridX, path[i].gridY - path[i - 1].gridY);

            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition); // Yeni bir yönde ise waypointi ekle
            }

            directionOld = directionNew; // Yönü güncelle
        }

        return waypoints.ToArray(); // Waypointleri dizi olarak döndür
    }

    // Ýki düðüm arasýndaki mesafeyi hesaplayan metot
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
