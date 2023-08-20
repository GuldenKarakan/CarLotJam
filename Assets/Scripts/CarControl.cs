using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CarControl : MonoBehaviour
{
    public Point[] point = new Point[2];
    public CustomColor color;
    public Transform door;


    private Vector3 selectedPoint;
    private int selectedPathLength = int.MaxValue;
    private UnityAction<Vector3> OnCalculated;
    private int totalCalculation;

    private void Start()
    {
        MeshRenderer renderer;
        foreach (Transform child in transform.GetChild(0))
        {
            renderer = child.GetComponent<MeshRenderer>();
            renderer.material = color.colorMaterial;
        }
    }

    public void CarPoint(GameObject player, UnityAction < Vector3 > onCalculated)
    {
        OnCalculated = onCalculated;
        selectedPathLength = int.MaxValue;
        totalCalculation = 0;
        for (int i = 0; i < point.Length; i++)
        {
            GameObject target = point[i].target;
            if (target != null)
            {
                PathRequestManager.RequestPath(player.transform.position, target.transform.position, OnPathCalculated);
                totalCalculation++;
            }
        }
    }

    private void OnPathCalculated(Vector3[] path, bool isSucces)
    {
        if(isSucces)
        {
            if(selectedPathLength > path.Length)
            {
                selectedPoint = path[path.Length - 1];
                selectedPathLength = path.Length;
            }
        }
        totalCalculation -= 1;
        if(totalCalculation <= 0)
        {
            OnCalculated.Invoke(Target());
        }
    }

    public Vector3 Target()
    {
        if (selectedPathLength != int.MaxValue)
            return selectedPoint;
        return point[0].transform.position;
    }

    public void PlayAnim()
    { 
        if (door != null)
            door.DORotate(new Vector3(0, 70, 0), .5f).SetEase(Ease.Linear).OnComplete(() => door.DORotate(Vector3.zero, .5f));
    }
}

