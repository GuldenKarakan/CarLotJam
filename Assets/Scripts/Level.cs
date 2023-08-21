using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [HideInInspector] public bool isFinish;
    public int carCount;

    public static Level instance;
    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
