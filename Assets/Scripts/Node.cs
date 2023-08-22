using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node s�n�f�n� IHeapItem aray�z� ile birle�tiren ve temel d���m yap�s�n� tan�mlayan s�n�f
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

    // Constructor - D���m �zellikleri atan�r
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    // D���m�n toplam tahmini maliyeti olan fCost'u hesaplayan �zellik
    public int fCost
    {
        get { return gCost + hCost; }
    }

    // �ncelikli kuyrukta d���m�n dizinini getirirken kullan�lan �zellik
    public int HeapIndex
    {
        get { return heapIndex; }
        set { heapIndex = value; }
    }

    // D���mleri �ncelikli kuyrukta kar��la�t�rmak i�in kullan�lan CompareTo metodu
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
