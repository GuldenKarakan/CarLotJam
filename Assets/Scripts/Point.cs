using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    [SerializeField] Transform door;
    [SerializeField] int cordinat;
    CarControl car;

    private void Start()
    {
        car = transform.parent.GetComponent<CarControl>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Floor floor = other.GetComponent<Floor>();
        Player player = other.GetComponent<Player>();

        if (floor != null)
            target = other.gameObject;

        if (player != null)
        {
            car.door = door;
            car.cordinat = cordinat;
        }
    }
}
