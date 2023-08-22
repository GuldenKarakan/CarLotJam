using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor : MonoBehaviour
{
    private Color originalColor;
    private Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
    }
    public void ChangeColor(Color color)
    {
        material.DOColor(color, .4f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            material.DOColor(originalColor, .4f).SetEase(Ease.InBounce);
        });
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
}
