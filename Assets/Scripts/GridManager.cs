using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [HideInInspector] public bool isChangingColor = false;
    public GameObject gridPointPrefab; // Izgara noktasý için prefab

    public int gridSizeX = 5; // Izgara boyutu X
    public int gridSizeZ = 5; // Izgara boyutu Z
    public float spacing = 1.0f; // Noktalar arasý mesafe

    private List<GameObject> gridPoints = new List<GameObject>();
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            gridPoints.Add(transform.GetChild(i).gameObject);
    }

    [ContextMenu("Create Grid")]
    void CreateGrid()
    {
        Vector3 spawnPosition = transform.position; // Parent objesinin baþlangýç noktasý
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 newPosition = spawnPosition + new Vector3(x * spacing, 0, z * spacing);
                GameObject gridPointGO = Instantiate(gridPointPrefab, newPosition, Quaternion.identity, transform);
            }
        }
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        for (int i = transform.childCount; i > 0; i--)
            DestroyImmediate(transform.GetChild(0).gameObject);
    }

    // Gizmos'u kullanarak oluþturulan grid'i editor üzerinde çizer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 gridSizeWorld = new Vector3(gridSizeX * spacing, 0.1f, gridSizeZ * spacing);
        Gizmos.DrawWireCube(transform.position + new Vector3(gridSizeX * spacing, 0, gridSizeZ * spacing) * 0.5f, gridSizeWorld);
    }
}
