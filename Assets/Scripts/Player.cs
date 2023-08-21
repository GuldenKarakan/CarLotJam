using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public SkinnedMeshRenderer body;
    public CustomColor color;
    public ParticleSystem angry, happy; 

    [SerializeField] private Material outLine;

    private void Start()
    {
        anim = GetComponent<Animator>();
        body = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        body.material = color.colorMaterial;
    }
    public void PlayAnim(Transform target)
    {
        transform.LookAt(target);
        transform.DOMove(target.position, .8f).OnComplete(() => transform.parent = target);
        transform.DOScale(Vector3.zero, 1f);
    }

    public IEnumerator OutlineAdd()
    {
        Material[] currentMaterials = body.materials;
        Material[] oldMaterials = currentMaterials;

        Material[] newMaterials = new Material[currentMaterials.Length + 1];
        currentMaterials.CopyTo(newMaterials, 0);

        newMaterials[currentMaterials.Length] = outLine;

        body.materials = newMaterials;

        yield return new WaitForSeconds(1f);
        body.materials = oldMaterials;

    }
}
