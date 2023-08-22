using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node sýnýfýný IHeapItem arayüzü ile birleþtiren ve temel düðüm yapýsýný tanýmlayan sýnýf
public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    int heapIndex;

    // Constructor - Düðüm özellikleri atanýr
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // Düðümün toplam tahmini maliyeti olan fCost'u hesaplayan özellik
    public int fCost
    {
        get { return gCost + hCost; }
    }

    // Öncelikli kuyrukta düðümün dizinini getirirken kullanýlan özellik
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    // Düðümleri öncelikli kuyrukta karþýlaþtýrmak için kullanýlan CompareTo metodu
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
