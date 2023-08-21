using UnityEngine;
using DG.Tweening;

public class Barrier : MonoBehaviour
{
    [SerializeField] Transform obje;
    [SerializeField] ParticleSystem finishParticle;
    private void OnTriggerEnter(Collider other)
    {
        CarControl car = other.GetComponent<CarControl>();
        if (car == null) return;

        obje.transform.DOLocalRotate(new Vector3(0, 0, 30), .15f);
        finishParticle.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        obje.transform.DOLocalRotate(Vector3.zero, .15f).SetDelay(.5f);
    }
}
