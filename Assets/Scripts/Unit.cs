using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe do�ru hareketini sa�layan s�n�f
public class Unit : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    private GameObject target;
    private GameObject player;
    private Floor floor;

    private Vector3[] path; // Yol noktalar�n� i�eren dizi
    private int targetIndex; // Hedef yol noktas�n�n dizinini tutan de�i�ken

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
                    if (player != null)
                    {
                        player.layer = 6;
                        player = null;
                    }

                    player = _player.gameObject;
                    player.layer = 8;
                    GetComponent<Grids>().CreatGrid();
                }

                if (floor != null && player != null && floor.gameObject.layer == 9)
                {
                    target = floor.gameObject;
                    PathRequestManager.RequestPath(player.transform.position, target.transform.position, OnPathFound);
                }
            }
        }
    }
    // Yol bulundu�unda �a�r�lan metot
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

    // Yolu takip eden i�lemi ger�ekle�tiren IEnumerator metodu
    IEnumerator FollowPath()
    {
        // Ba�lang��ta, birim nesne ilk yol noktas�na hareket etmeye ba�lar
        Vector3 currentWaypoint = path[0];

        // Sonsuz bir d�ng� ba�lat�l�r, ��nk� birim nesne hedefe vard���nda d�ng� sonlanacak
        while (true)
        {
            // E�er birim nesne �u anki yol noktas�na vard�ysa
            if (player.transform.position == currentWaypoint)
            {
                // Hedef yol noktas�n�n dizinini art�r
                targetIndex++;

                // E�er hedef dizini yol noktalar� dizininin boyutuna ula�t�ysa
                if (targetIndex >= path.Length)
                {
                    yield return new WaitForSeconds(.5f);
                    // Yolun sonuna gelindi, d�ng�y� sonland�r
                    player.layer = 6;
                    player = null;
                    GetComponent<Grids>().CreatGrid();
                    yield break;
                }

                // Yeni hedef yol noktas�n� belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi do�ru y�ne hareket ettir
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint, speed * Time.deltaTime);

            // Bir sonraki frame'e ge�meden �nce bekle
            yield return null;
        }
    }
}

