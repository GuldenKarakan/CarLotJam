using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public CustomColor color;

    private void Start()
    {

        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = color.colorMaterial;
    }
    public void PlayAnim(Transform target)
    {
        transform.DOMove(target.position, .5f).OnComplete(() => transform.parent = target);
        transform.DOScale(Vector3.zero, .8f);
    }
}
