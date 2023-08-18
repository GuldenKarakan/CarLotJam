using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Node s�n�f�n� IHeapItem aray�z� ile birle�tiren ve temel d���m yap�s�n� tan�mlayan s�n�f
public class Node : IHeapItem<Node>
{
    public bool walkable; // Bu d���m�n y�r�nebilir olup olmad���n� belirten boolean de�er
    public Vector3 worldPosition; // D���m�n d�nya koordinatlar�
    public int gridX; // Izgarada d���m�n X koordinat�
    public int gridY; // Izgarada d���m�n Y koordinat�

    public int gCost; // Ba�lang�� d���m�nden bu d���me olan ger�ek maliyet
    public int hCost; // Bu d���mden hedef d���me olan tahmini maliyet
    public Node parent; // Bu d���m�n �nceki d���m� (yolu takip etmek i�in)

    int heapIndex; // �ncelikli kuyruk yap�s�nda dizin tutmak i�in kullan�l�r

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
        return -compare;// �ncelikli kuyru�un en y�ksek �ncelikli ��esinin en �stte olmas� i�in ters s�ralama yap�l�r
    }
}
