using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    private Color originalColor;
    private GridManager gridManager;

    private void Start()
    {
        gridManager = transform.parent.parent.GetComponent<GridManager>();
        originalColor = GetComponent<Renderer>().material.color;
    }

    private void OnMouseDown()
    {
        if (!gridManager.isChangingColor) // Renk deðiþimi devam etmiyorsa ve iþgal edilmemiþse
        {
            gridManager.isChangingColor = true; // Renk deðiþimi baþladý
            gridManager.clickedGrid = transform;
            // A* algoritmasýný burada kullanarak yolun varlýðýný kontrol edin
            bool hasPath = gridManager.CheckPathAvailability(); // Bu kýsmý kendi A* algoritmanýza göre düzenlemelisiniz

            if (hasPath)
            {
                StartCoroutine(ChangeColorTemporarily(Color.green, 0.35f, 0.2f)); // Yeþil yanýp sönecek
            }
            else
            {
                StartCoroutine(ChangeColorTemporarily(Color.red, 0.35f, 0.2f)); // Kýrmýzý yanýp sönecek
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

        gridManager.isChangingColor = false; // Renk deðiþimi bitti
    }

    private void ChangeColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}