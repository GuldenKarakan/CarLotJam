using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [HideInInspector] public bool isChangingColor = false;
    [HideInInspector] public Transform clickedGrid;
    public Transform characterGrid;
    public GameObject gridPointPrefab; // Izgara noktas� i�in prefab

    public int gridSizeX = 5; // Izgara boyutu X
    public int gridSizeZ = 5; // Izgara boyutu Z
    public float spacing = 1.0f; // Noktalar aras� mesafe

    private List<GameObject> gridPoints = new List<GameObject>();
    private void Start()
    {
        gridPoints = new List<GameObject>(GetComponentsInChildren<GameObject>());
    }

    [ContextMenu("Create Grid")]
    void CreateGrid()
    {
        Vector3 spawnPosition = transform.position; // Parent objesinin ba�lang�� noktas�
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 newPosition = spawnPosition + new Vector3(x * spacing, 0, z * spacing);
                Instantiate(gridPointPrefab, newPosition, Quaternion.identity, transform);
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        for (int i = transform.childCount; i > 0; i--)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    // A* algoritmas�n� kullanarak yolun varl���n� kontrol edin
    public bool CheckPathAvailability()
    {

        return false;
    }

    // Gizmos'u kullanarak olu�turulan grid'i editor �zerinde �izer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 gridSizeWorld = new Vector3(gridSizeX * spacing, 0.1f, gridSizeZ * spacing);
        Gizmos.DrawWireCube(transform.position + new Vector3(gridSizeX * spacing, 0, gridSizeZ * spacing) * 0.5f, gridSizeWorld);
    }
}
