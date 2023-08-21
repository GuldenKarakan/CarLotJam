using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe do�ru hareketini sa�layan s�n�f
public class Unit : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    private GameObject target;
    private Player player;
    private Floor floor;
    private CarControl carControl;

    private Vector3[] path; // Yol noktalar�n� i�eren dizi
    private int targetIndex; // Hedef yol noktas�n�n dizinini tutan de�i�ken

    private IEnumerator currentPath;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = null;
            carControl = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                floor = hit.transform.GetComponent<Floor>();
                Player _player = hit.transform.GetComponent<Player>();
                carControl = hit.transform.GetComponent<CarControl>();

                if (_player != null)
                {
                    if (player != null)
                    {
                        player.gameObject.layer = 6;
                        player = null;
                    }

                    player = _player;
                    player.anim.SetTrigger("happy");
                    player.gameObject.layer = 8;
                    GetComponent<Grids>().CreatGrid();
                }

                if (floor != null && player != null)
                {
                    target = floor.gameObject;
                    RequestPath(target.transform.position);
                }

                if (carControl != null && carControl.color.colorName == player.color.colorName)
                {
                    carControl.CarPoint(player.gameObject, (x) => RequestPath(x));
                    player.happy.Play();
                }
            }
        }
    }

    private void RequestPath(Vector3 targetPos)
    {
        PathRequestManager.RequestPath(player.transform.position, targetPos, OnPathFound);
    }
    // Yol bulundu�unda �a�r�lan metot
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (currentPath != null)
                StopCoroutine(currentPath);
            if (newPath.Length > 0)
            {
                currentPath = FollowPath();
                targetIndex = 0;
                StartCoroutine(currentPath);
                if (target != null)
                    floor.ChangeColor(Color.green);

                player.anim.SetTrigger("run");
            }

        }
        else
        
        if (target != null)
        {
            floor.ChangeColor(Color.red);
            player.angry.Play();
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
            if (player.transform.position == currentWaypoint)
            {
                // Hedef yol noktas�n�n dizinini art�r
                targetIndex++;

                // E�er hedef dizini yol noktalar� dizininin boyutuna ula�t�ysa
                if (targetIndex >= path.Length)
                {
                    if (carControl != null)
                    {
                        player.GetComponent<Collider>().enabled = false;
                        carControl.PlayAnim(1);
                        yield return new WaitForSeconds(.1f);
                        player.anim.SetTrigger("openDoor");
                        carControl.PlayAnim(2);
                        player.PlayAnim(carControl.animPos);
                    }
                    else
                        yield return new WaitForSeconds(.2f);
                    // Yolun sonuna gelindi, d�ng�y� sonland�r
                    player.gameObject.layer = 6;
                    player = null;
                    GetComponent<Grids>().CreatGrid();
                    yield break;
                }

                // Yeni hedef yol noktas�n� belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi do�ru y�ne hareket ettir
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint, speed * Time.deltaTime);
            //Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint - player.transform.position);
            //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, speed *2f * Time.deltaTime);
            player.transform.LookAt(currentWaypoint);

            // Bir sonraki frame'e ge�meden �nce bekle
            yield return null;
        }
    }
}

