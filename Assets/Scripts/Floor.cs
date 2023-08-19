using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public bool walkable = false;
    public bool isTarget = false;
    public void ChangeColor(Color color)
    {
        if (gameObject.layer != LayerMask.NameToLayer("Unclickable"))
        {
            GetComponent<Renderer>().material.color = color;
        }
    }
}
