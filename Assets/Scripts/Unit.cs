using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe doðru hareketini saðlayan sýnýf
public class Unit : MonoBehaviour
{
    public Transform target;
    [SerializeField] float speed = 10f;

    Vector3[] path; // Yol noktalarýný içeren dizi
    int targetIndex; // Hedef yol noktasýnýn dizinini tutan deðiþken

    private void Update()
    {
        if (Input.GetKeyDown("space"))
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    // Yol bulunduðunda çaðrýlan metot
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    // Yolu takip eden iþlemi gerçekleþtiren IEnumerator metodu
    IEnumerator FollowPath()
    {
        // Baþlangýçta, birim nesne ilk yol noktasýna hareket etmeye baþlar
        Vector3 currentWaypoint = path[0];

        // Sonsuz bir döngü baþlatýlýr, çünkü birim nesne hedefe vardýðýnda döngü sonlanacak
        while (true)
        {
            // Eðer birim nesne þu anki yol noktasýna vardýysa
            if (transform.position == currentWaypoint)
            {
                // Hedef yol noktasýnýn dizinini artýr
                targetIndex++;

                // Eðer hedef dizini yol noktalarý dizininin boyutuna ulaþtýysa
                if (targetIndex >= path.Length)
                {
                    // Yolun sonuna gelindi, döngüyü sonlandýr
                    yield break;
                }

                // Yeni hedef yol noktasýný belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi doðru yöne hareket ettir
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            // Bir sonraki frame'e geçmeden önce bekle
            yield return null;
        }
    }
}
