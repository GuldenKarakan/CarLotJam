using System.Collections;
using UnityEngine;
using System;

// �ncelikli kuyruk yap�s�n� tan�mlayan s�n�f
public class Heap<T> where T : IHeapItem<T>
{
    // ��elerin sakland��� dizi ve ��e say�s�n� tutan de�i�ken
    T[] items;
    int currentItemCount;

    // Constructor - Maksimum ��e say�s� belirlenir ve dizi olu�turulur
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // ��e ekleme metodu
    public void Add(T item)
    {
        // ��enin kendi HeapIndex de�eri ayarlan�r
        item.HeapIndex = currentItemCount;
        // ��e diziyi ilgili indekse eklenir
        items[currentItemCount] = item;
        // Yeni ��eyi yukar� do�ru s�ralama i�lemi yap�l�r
        SortUp(item);
        // ��e say�s� art�r�l�r
        currentItemCount++;
    }

    // �lk ��eyi ��karma metodu
    public T RemoveFirst()
    {
        // �lk ��e al�n�r
        T firstItem = items[0];
        // ��e say�s� azalt�l�r
        currentItemCount--;
        // Son ��e ilk ��e olarak ta��n�r
        items[0] = items[currentItemCount];
        // Ta��nan ��enin HeapIndex de�eri g�ncellenir
        items[0].HeapIndex = 0;
        // ��eyi a�a�� do�ru s�ralama i�lemi yap�l�r
        SortDown(items[0]);
        // �lk ��e d�nd�r�l�r
        return firstItem;
    }


    // ��enin g�ncellenmesi metodu
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // ��e say�s�n� d�nd�ren �zellik
    public int Count
    {
        get { return currentItemCount; }
    }
    public bool Contains(T item)
    {
        // �lgili ��enin HeapIndex de�eri kullan�larak kontrol yap�l�r
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item)
    {
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else return;
            }
            else return;
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else break;

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // �ki ��eyi yer de�i�tiren metot
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// IHeapItem aray�z� ile ��eleri kar��la�t�r�labilir hale getiren aray�z
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
