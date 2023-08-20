using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Birim nesnesinin hedefe doðru hareketini saðlayan sýnýf
public class Unit : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    private GameObject target;
    private Player player;
    private Floor floor;
    private CarControl carControl;

    private Vector3[] path; // Yol noktalarýný içeren dizi
    private int targetIndex; // Hedef yol noktasýnýn dizinini tutan deðiþken

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
                    player.gameObject.layer = 8;
                    GetComponent<Grids>().CreatGrid();
                }

                if (floor != null && player != null)
                {
                    target = floor.gameObject;
                    RequestPath(target.transform.position);
                }

                if(carControl != null && carControl.color.colorName == player.color.colorName)
                {
                   carControl.CarPoint(player.gameObject, (x) => RequestPath(x));
                }
            }
        }
    }

    private void RequestPath(Vector3 targetPos)
    {
        PathRequestManager.RequestPath(player.transform.position, targetPos, OnPathFound);
    }
    // Yol bulunduðunda çaðrýlan metot
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            if (currentPath != null)
                StopCoroutine(currentPath);
            if(newPath.Length > 0)
            {
                currentPath = FollowPath();
                targetIndex = 0;
                StartCoroutine(currentPath);
                if (target != null)
                    floor.ChangeColor(Color.green);
            }
           
        }
        else
            if (target != null)
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
                    yield return new WaitForSeconds(.5f);
                    // Yolun sonuna gelindi, döngüyü sonlandýr
                    if (carControl != null)
                    {
                        carControl.PlayAnim();
                        player.PlayAnim(carControl.transform);
                        player.GetComponent<Collider>().enabled = false;
                    }

                    player.gameObject.layer = 6;
                    player = null;
                    GetComponent<Grids>().CreatGrid();
                    yield break;
                }

                // Yeni hedef yol noktasýný belirle
                currentWaypoint = path[targetIndex];
            }

            // Birim nesneyi doðru yöne hareket ettir
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentWaypoint, speed * Time.deltaTime);
            //Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint - player.transform.position);
            //player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, speed *2f * Time.deltaTime);
            player.transform.LookAt(currentWaypoint);

            // Bir sonraki frame'e geçmeden önce bekle
            yield return null;
        }
    }
}

