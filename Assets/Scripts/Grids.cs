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
    private void Update()
    {
        if (Input.GetKey("a"))
            CreatGrid();
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }


    [ContextMenu("Creat Grid")]
    private void CreatGrid()
    {
        ClearGrid();

        // �zgaran�n d�nya boyutlar�n� hesapla
        gridWorldSize = new Vector2(gridAxisX, gridAxisY) * 2;
        nodeDiameter = nodeRadius * 2;

        // Izgara boyutlar�n� d���m �ap�na g�re hesapla
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Izgara d���mleri dizisini olu�tur
        grid = new Node[gridSizeX, gridSizeY];

        // Izgaran�n sol alt k��esini hesapla
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // Her bir d���m� olu�tur ve d���m�n y�r�n�lebilirlik durumunu kontrol et
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                // D���m�n d�nya konumunu hesapla
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);

                // D���m�n �zerinde engel var m� kontrol et
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // D���m� olu�tur ve izgara dizisine ekle
                grid[i, j] = new Node(walkable, worldPoint, i, j);
                GameObject floor = Instantiate(gridObject, worldPoint, Quaternion.identity, transform);
                floor.layer = walkable ? 0 : 7;

            }
        }
    }

    [ContextMenu("Clear Grid")]
    private void ClearGrid()
    {
        // Olu�turulan Gridleri editorden temizler
        for (int i = transform.childCount; i > 0; i--)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();


        // D���m�n etraf�ndaki kom�ular� dola�
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                // Kom�u d���m�n dizinlerini hesapla
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Dizinler izgara s�n�rlar�nda m� kontrol et
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    // Kom�u d���m� kom�ular listesine ekle
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;

    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // D�nya koordinatlar�n� izgara i�indeki koordinatlara d�n��t�r
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // D���m dizinlerini hesapla
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // �lgili d���m� d�nd�r
        return grid[x, y];
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
            }
        }
    }
}
