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
        if (gameObject.layer != 7)
        {
            material.DOColor(color, .4f).SetEase(Ease.OutBounce).OnComplete(ResetColor); ;
        }
    }
    private void ResetColor()
    {
        // Tween tamamland���nda objeyi �zg�n rengine geri tweenle
        material.DOColor(originalColor, .4f).SetEase(Ease.InBounce);
    }

    private void OnMouseDown()
    {
        gameObject.layer = Physics.CheckSphere(transform.position, 1f, 8) ? 7 : 9;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
