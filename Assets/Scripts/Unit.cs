using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe doðru hareketini saðlayan sýnýf
public class Unit : MonoBehaviour
{
    GameObject target;
    public GameObject player;
    [SerializeField] float speed = 10f;
    Floor floor;

    Vector3[] path; // Yol noktalarýný içeren dizi
    int targetIndex; // Hedef yol noktasýnýn dizinini tutan deðiþken

    private IEnumerator currentPath;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                floor = hit.transform.GetComponent<Floor>();
                Player _player = hit.transform.GetComponent<Player>();
                if (_player != null) 
                {
                    player = _player.gameObject;
                    player.layer = 0;
                    GetComponent<Grids>().CreatGrid();
                }

                if (floor != null && player != null)
                {
                    target = floor.gameObject;

                    PathRequestManager.RequestPath(player.transform.position, target.transform.position, OnPathFound);
                }
            }
        }
    }

    // Yol bulunduðunda çaðrýlan metot
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            if (currentPath != null)
                StopCoroutine(currentPath);
            currentPath = FollowPath();
            targetIndex = 0;
            StartCoroutine(currentPath);
            floor.ChangeColor(Color.green);
        }
        else
            floor.ChangeColor(Color.red);
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
            if (player.transform.position == currentWaypoint)
            {
                // Hedef yol noktasýnýn dizinini artýr
                targetIndex++;

                // Eðer hedef dizini yol noktalarý dizininin boyutuna ulaþtýysa
                if (targetIndex >= path.Length)
                {
                    // Yolun sonuna gelindi, döngüyü sonlandýr
                    player.layer = 6;
                    player = null;
                    GetComponent<Grids>().CreatGrid();
                    yield break;
                }

                // Yeni hedef yol noktasýný belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi doðru yöne hareket ettir
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint, speed * Time.deltaTime);

            // Bir sonraki frame'e geçmeden önce bekle
            yield return null;
        }
    }
}
