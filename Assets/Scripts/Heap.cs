using System.Collections;
using UnityEngine;
using System;

// Öncelikli kuyruk yapýsýný tanýmlayan sýnýf
public class Heap<T> where T : IHeapItem<T>
{
    // Öðelerin saklandýðý dizi ve öðe sayýsýný tutan deðiþken
    T[] items;
    int currentItemCount;

    // Constructor - Maksimum öðe sayýsý belirlenir ve dizi oluþturulur
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // Öðe ekleme metodu
    public void Add(T item)
    {
        // Öðenin kendi HeapIndex deðeri ayarlanýr
        item.HeapIndex = currentItemCount;
        // Öðe diziyi ilgili indekse eklenir
        items[currentItemCount] = item;
        // Yeni öðeyi yukarý doðru sýralama iþlemi yapýlýr
        SortUp(item);
        // Öðe sayýsý artýrýlýr
        currentItemCount++;
    }

    // Ýlk öðeyi çýkarma metodu
    public T RemoveFirst()
    {
        // Ýlk öðe alýnýr
        T firstItem = items[0];
        // Öðe sayýsý azaltýlýr
        currentItemCount--;
        // Son öðe ilk öðe olarak taþýnýr
        items[0] = items[currentItemCount];
        // Taþýnan öðenin HeapIndex deðeri güncellenir
        items[0].HeapIndex = 0;
        // Öðeyi aþaðý doðru sýralama iþlemi yapýlýr
        SortDown(items[0]);
        // Ýlk öðe döndürülür
        return firstItem;
    }


    // Öðenin güncellenmesi metodu
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    // Öðe sayýsýný döndüren özellik
    public int Count
    {
        get { return currentItemCount; }
    }
    public bool Contains(T item)
    {
        // Ýlgili öðenin HeapIndex deðeri kullanýlarak kontrol yapýlýr
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

    // Ýki öðeyi yer deðiþtiren metot
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// IHeapItem arayüzü ile öðeleri karþýlaþtýrýlabilir hale getiren arayüz
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
