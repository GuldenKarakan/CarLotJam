using UnityEngine;

public class SlotSpawner : MonoBehaviour
{
    [System.Flags]
    private enum Layout { Horizontal = 1, Vertical = 2 }
    [SerializeField] private Layout layout;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount;
    [SerializeField] private Vector2 space;
    [SerializeField] private Vector2Int direction;
    [SerializeField, Tooltip("Type 0 for infinite")] private Vector2Int gridSize;

    private void Start()
    {
        SpawnSlots(transform);
    }

    #region GIZMOS
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 maxPos = Vector3.zero;
        if (gridSize.x > 0)
            maxPos.x = gridSize.x;

        if (gridSize.x > 0)
            maxPos.z = Mathf.CeilToInt((float)slotCount / gridSize.x);
        else if (gridSize.y > 0)
            maxPos.z = gridSize.y;

        maxPos.x *= direction.x + space.x;
        maxPos.z *= direction.y + space.y;

        Vector3 dirFloat = new Vector3(direction.x, 0, direction.y);
        Vector3 center = transform.position + ((maxPos - dirFloat) / 2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, maxPos);
    }
#endif
    #endregion
    public void SpawnSlots(Transform slotParent)
    {
        if (layout == 0)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Please set at least one layout on " + transform.name, transform);
#endif
            return;
        }

        Vector2Int dir = new Vector2Int(layout.HasFlag(Layout.Horizontal) ? direction.x : 0, layout.HasFlag(Layout.Vertical) ? direction.y : 0);

        for (int i = 0; i < slotCount; i++)
        {
            GameObject clone = Instantiate(slotPrefab, slotParent);

            int indX = i;
            if (gridSize.x > 0)
                indX = i % gridSize.x;

            int indY = i;
            if (gridSize.x > 0)
                indY = i / gridSize.x;
            else if (gridSize.y > 0)
                indY = i % gridSize.y;
            Vector3 pos = new Vector3(indX * (dir.x + space.x), 0f, indY * (dir.y + space.y));

            clone.transform.localPosition = pos;
        }
    }
}
