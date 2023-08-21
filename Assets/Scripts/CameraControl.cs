using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform camPos1;
    [SerializeField] private Transform camPos2;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();

    }

    private void Update()
    {
        int gridY = Grids.instance.gridAxisY;
        if (gridY < 6)
        {
            transform.position = camPos1.position;
            transform.rotation = camPos1.rotation;
            cam.orthographic = false;
            cam.fieldOfView = 53;
        }
        else
        {
            transform.position = camPos2.position;
            transform.rotation = camPos2.rotation;
            cam.orthographic = true;
            cam.orthographicSize = 15f;
        }
    }
}
