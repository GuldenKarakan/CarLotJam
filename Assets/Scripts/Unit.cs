using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe do�ru hareketini sa�layan s�n�f
public class Unit : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed = 10f;

    Vector3[] path; // Yol noktalar�n� i�eren dizi
    int targetIndex; // Hedef yol noktas�n�n dizinini tutan de�i�ken

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    // Yol bulundu�unda �a�r�lan metot
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    // Yolu takip eden i�lemi ger�ekle�tiren IEnumerator metodu
    IEnumerator FollowPath()
    {
        // Ba�lang��ta, birim nesne ilk yol noktas�na hareket etmeye ba�lar
        Vector3 currentWaypoint = path[0];

        // Sonsuz bir d�ng� ba�lat�l�r, ��nk� birim nesne hedefe vard���nda d�ng� sonlanacak
        while (true)
        {
            // E�er birim nesne �u anki yol noktas�na vard�ysa
            if (transform.position == currentWaypoint)
            {
                // Hedef yol noktas�n�n dizinini art�r
                targetIndex++;

                // E�er hedef dizini yol noktalar� dizininin boyutuna ula�t�ysa
                if (targetIndex >= path.Length)
                {
                    // Yolun sonuna gelindi, d�ng�y� sonland�r
                    yield break;
                }

                // Yeni hedef yol noktas�n� belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi do�ru y�ne hareket ettir
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            // Bir sonraki frame'e ge�meden �nce bekle
            yield return null;
        }
    }
}
