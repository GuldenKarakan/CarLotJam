using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    public GameObject gridObject;
    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    public int gridAxisX, gridAxisY;
    public float nodeRadius;
    Node[,] grid;

    private Vector2 gridWorldSize;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake()
    {
        CreatGrid();
    }
    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    [ContextMenu("Creat Grid")]
    public void CreatGrid()
    {
        ClearGrid();

        // Ýzgaranýn dünya boyutlarýný hesapla
        gridWorldSize = new Vector2(gridAxisX, gridAxisY) * 2;
        nodeDiameter = nodeRadius * 2;

        // Izgara boyutlarýný düðüm çapýna göre hesapla
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Izgara düðümleri dizisini oluþtur
        grid = new Node[gridSizeX, gridSizeY];

        // Izgaranýn sol alt köþesini hesapla
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // Her bir düðümü oluþtur ve düðümün yürünülebilirlik durumunu kontrol et
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                // Düðümün dünya konumunu hesapla
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);

                // Düðümün üzerinde engel var mý kontrol et
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // Düðümü oluþtur ve izgara dizisine ekle
                grid[i, j] = new Node(walkable, worldPoint, i, j);
                GameObject floor = Instantiate(gridObject, worldPoint, Quaternion.identity, transform);
                floor.layer = walkable ? 9 : 7;
            }
        }
    }

    [ContextMenu("Clear Grid")]
    private void ClearGrid()
    {
        // Oluþturulan Gridleri editorden temizler
        for (int i = transform.childCount; i > 0; i--)
            DestroyImmediate(transform.GetChild(0).gameObject);

        ResetGrid();
    }

    public void ResetGrid()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                grid[i, j].gCost = 0;
                grid[i, j].hCost = 0;
                grid[i, j].parent = null;
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        if (IsNeighbourValid(node.gridX - 1, node.gridY))
            neighbours.Add(grid[node.gridX - 1, node.gridY]);

        if (IsNeighbourValid(node.gridX + 1, node.gridY))
            neighbours.Add(grid[node.gridX + 1, node.gridY]);

        if (IsNeighbourValid(node.gridX, node.gridY -1))
            neighbours.Add(grid[node.gridX, node.gridY -1]);

        if (IsNeighbourValid(node.gridX, node.gridY +1))
            neighbours.Add(grid[node.gridX, node.gridY +1]);

        return neighbours;

    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Dünya koordinatlarýný izgara içindeki koordinatlara dönüþtür
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // Düðüm dizinlerini hesapla
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // Ýlgili düðümü döndür
        return grid[x, y];
    }

    private bool IsNeighbourValid(int checkX, int checkY)
    {
        return checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridAxisX * 2, 1, gridAxisY * 2));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, new Vector3(1, .1f, 1) * (nodeDiameter - .1f));
                if(n.parent != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(n.worldPosition + Vector3.up, n.parent.worldPosition + Vector3.up * 2);
                }
            }
        }
    }

}
