using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public CustomColor color;
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = color.colorMaterial;
    }
    public void PlayAnim(Transform target)
    {
        transform.LookAt(target);
        transform.DOMove(target.position, .8f).OnComplete(() => transform.parent = target);
        transform.DOScale(Vector3.zero, 2f);
    }
}
