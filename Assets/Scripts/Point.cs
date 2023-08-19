using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject target;
    private void OnTriggerEnter(Collider other)
    {
        Floor floor = other.GetComponent<Floor>();
        if (floor == null)
            return;
        if (floor.gameObject.layer != 7)
            target = other.gameObject;
    }
}
