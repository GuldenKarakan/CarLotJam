using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private Color originalColor;
    private GridManager gridManager;

    private bool isOccupied = false;
    private GameObject occupyingObject; // ��gal eden obje
    private bool isBlocked = false; // Engellenmi� mi?

    public bool IsBlocked
    {
        get { return isBlocked; }
    }

    private void Start()
    {
        gridManager = transform.parent.parent.GetComponent<GridManager>();
        originalColor = GetComponent<Renderer>().material.color;
    }

    private void OnMouseDown()
    {
        if (!gridManager.isChangingColor && !isOccupied) // Renk de�i�imi devam etmiyorsa ve i�gal edilmemi�se
        {
            gridManager.isChangingColor = true; // Renk de�i�imi ba�lad�
            // A* algoritmas�n� burada kullanarak yolun varl���n� kontrol edin
            bool hasPath = CheckPathAvailability(); // Bu k�sm� kendi A* algoritman�za g�re d�zenlemelisiniz

            if (hasPath)
            {
                StartCoroutine(ChangeColorTemporarily(Color.green, 0.35f, 0.2f)); // Ye�il yan�p s�necek
            }
            else
            {
                StartCoroutine(ChangeColorTemporarily(Color.red, 0.35f, 0.2f)); // K�rm�z� yan�p s�necek
            }
        }
    }

    private IEnumerator ChangeColorTemporarily(Color color, float duration1, float duration2)
    {
        ChangeColor(color);
        yield return new WaitForSeconds(duration1);
        ChangeColor(originalColor);
        yield return new WaitForSeconds(duration2);
        ChangeColor(color);
        yield return new WaitForSeconds(duration1);
        ChangeColor(originalColor);

        gridManager.isChangingColor = false; // Renk de�i�imi bitti
    }

    private void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    // A* algoritmas�n� kullanarak yolun varl���n� kontrol edin
    private bool CheckPathAvailability()
    {
        // Burada A* algoritmas�na g�re yolun varl���n� kontrol edin
        // E�er yol varsa true, yoksa false d�nd�r�n
        // �rnek bir kontrol yap�s�:
        // return Random.Range(0, 2) == 0; // Rastgele true veya false d�nd�r�r
        return false; // �rnek olarak her zaman true d�nd�r�yoruz
    }
}