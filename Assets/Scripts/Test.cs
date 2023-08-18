using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject gridObjectPrefab; // Kullanýlacak grid objesi prefabý

    public GameObject startCubePrefab; // Baþlangýç noktasý için küp prefabý
    public GameObject targetCubePrefab; // Hedef noktasý için küp prefabý

    public float spacing = 2f; // Hücreler arasýndaki boþluk
    public float moveSpeed = .5f; // hýzz


    private GameObject startCube; // Baþlangýç noktasý küpü
    private GameObject targetCube; // Hedef noktasý küpü

    private List<Vector3> availableGridPositions = new List<Vector3>();
    private List<Vector3> path = new List<Vector3>(); // Yolu saklayacak liste
    private void Start()
    {
        CreateGrid();
        PlaceCubes();
        CalculatePath();
        MoveStartCube();
    }

    void CreateGrid()
    {
        Vector3 startSpawnPosition = transform.position - new Vector3(spacing * 2, 0f, spacing * 2);

        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                Vector3 spawnPosition = startSpawnPosition + new Vector3(x * spacing, 0f, z * spacing);
                availableGridPositions.Add(spawnPosition);
                Instantiate(gridObjectPrefab, spawnPosition, Quaternion.identity, transform);
            }
        }
    }

    void PlaceCubes()
    {
        int startCubeIndex = Random.Range(0, availableGridPositions.Count);
        startCube = Instantiate(startCubePrefab, availableGridPositions[startCubeIndex], Quaternion.identity);
        availableGridPositions.RemoveAt(startCubeIndex);

        int targetCubeIndex = Random.Range(0, availableGridPositions.Count);
        targetCube = Instantiate(targetCubePrefab, availableGridPositions[targetCubeIndex], Quaternion.identity);
        availableGridPositions.RemoveAt(targetCubeIndex);
    }
    void CalculatePath()
    {
        path = ChooseShortestPath();
    }

    List<Vector3> ChooseShortestPath()
    {
        List<Vector3> shortestPath = new List<Vector3>();
        float shortestDistance = float.MaxValue;

        foreach (Vector3 position in availableGridPositions)
        {
            float distance = Vector3.Distance(position, targetCube.transform.position);
            if (distance < shortestDistance && !IsCornerBlocked(position))
            {
                shortestDistance = distance;
                shortestPath.Clear();
                shortestPath.Add(position);
            }
        }

        return shortestPath;
    }

    bool IsCornerBlocked(Vector3 position)
    {
        Vector3[] offsets = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        foreach (Vector3 offset in offsets)
        {
            Vector3 neighbor = position + offset * spacing;

            if (!availableGridPositions.Contains(neighbor))
            {
                bool isBlocked = true;

                if (offset == Vector3.forward || offset == Vector3.back)
                {
                    isBlocked = !availableGridPositions.Contains(position + Vector3.left * spacing) || !availableGridPositions.Contains(position + Vector3.right * spacing);
                }
                else if (offset == Vector3.left || offset == Vector3.right)
                {
                    isBlocked = !availableGridPositions.Contains(position + Vector3.forward * spacing) || !availableGridPositions.Contains(position + Vector3.back * spacing);
                }

                if (isBlocked)
                {
                    return true;
                }
            }
        }

        return false;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 point in path)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }

        if (path.Count > 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startCube.transform.position, path[0]); // Baþlangýç küpünden yola baþla
            Gizmos.DrawLine(path[path.Count - 1], targetCube.transform.position); // Yoldan hedef küpe devam et
        }
    }

    // Baþlangýç küpünü hareket ettir
    void MoveStartCube()
    {
        StartCoroutine(MoveCubeAlongPath());
    }

    // Yolu takip ederek küpü hareket ettir
    IEnumerator MoveCubeAlongPath()
    {
        foreach (Vector3 pathPoint in path)
        {
            Vector3 targetPosition = pathPoint;
            while (Vector3.Distance(startCube.transform.position, targetPosition) > 0.01f)
            {
                startCube.transform.position = Vector3.MoveTowards(startCube.transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}