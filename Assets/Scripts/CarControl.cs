using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class CarControl : MonoBehaviour
{
    [HideInInspector] public Transform door;
    [HideInInspector] public int cordinat;

    public Point[] point = new Point[2];
    public CustomColor color;
    public Transform animPos;
    public ParticleSystem happy;

    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private LayerMask layer;
    [SerializeField] private Transform body;
    [SerializeField] private Material outLine;

    private Vector3 selectedPoint;
    private UnityAction<Vector3> OnCalculated;
    private MeshRenderer bodyMaterial;
    private Grids grid;
    private int selectedPathLength = int.MaxValue;
    private int totalCalculation;
    private float speed = 8f;
    private bool isMove = false;
    int value = 0;
    private List<GameObject> roads = new List<GameObject>();

    private void Start()
    {
        grid = Grids.instance;
        bodyMaterial = body.GetComponent<MeshRenderer>();
        MeshRenderer renderer;
        smoke.Stop();
        foreach (Transform child in transform.GetChild(0))
        {
            renderer = child.GetComponent<MeshRenderer>();
            renderer.material = color.colorMaterial;
        }
    }

    private void Update()
    {
        if (!isMove)
        {
            RaycastHit front, back;
            Physics.Raycast(transform.position, transform.forward, out front, 15, layer);
            Physics.Raycast(transform.position, -transform.forward, out back, 15, layer);
            if (front.transform != null && back.transform != null && front.transform.gameObject.layer == 10 && back.transform.gameObject.layer == 10)
            {
                if (front.distance < back.distance)
                    value = 1;
                else
                    value = -1;

                isMove = true;
                RayContact();
            }
            else if (front.transform != null && front.transform.gameObject.layer == 10)
            {
                value = 1;
                isMove = true;
                RayContact();
            }
            else if (back.transform != null && back.transform.gameObject.layer == 10)
            {
                value = -1;
                isMove = true;
                RayContact();
            }
        }
        else
        {
            transform.Translate(Vector3.forward * value * speed * Time.deltaTime);
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

    public Vector3 Target()
    {
        if (selectedPathLength != int.MaxValue)
            return selectedPoint;
        return point[0].transform.position;
    }

    public void PlayAnim(int animIndex)
    {
        if (door == null)
            return;

        switch (animIndex)
        {
            case 1:
                door.DOLocalRotate(new Vector3(0, 70, 0) * cordinat, .4f).SetEase(Ease.Linear).OnComplete(() => door.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.Linear));
                break;
            case 2:
                body.DOLocalRotate(new Vector3(0, 0, 18) * cordinat, .4f).SetEase(Ease.Linear).OnComplete(() => body.DOLocalRotate(Vector3.zero, .4f).SetEase(Ease.Linear));
                break;
            case 3:
                body.DOShakePosition(.2f, .03f).SetEase(Ease.Linear).SetLoops(-1);
                break;
            default:
                break;



        }
    }

    public IEnumerator OutlineAdd()
    {
        Material[] currentMaterials = bodyMaterial.materials;
        Material[] oldMaterials = currentMaterials;

        Material[] newMaterials = new Material[currentMaterials.Length + 1];
        currentMaterials.CopyTo(newMaterials, 0);

        newMaterials[currentMaterials.Length] = outLine;

        bodyMaterial.materials = newMaterials;

        yield return new WaitForSeconds(1f);
        bodyMaterial.materials = oldMaterials;

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

    private void RayContact()
    {
        GetComponent<BoxCollider>().size = Vector3.one * .5f;
        gameObject.layer = 0;
        grid.CreatGrid();
    }

    private void Rotate(GameObject takenRoad)
    {
        Vector3 rotation = transform.rotation.eulerAngles + new Vector3(0, 90, 0) * value;
        transform.DOLocalRotate(rotation, .1f).SetEase(Ease.Linear);
        //value = 1;
        roads.Add(takenRoad);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (roads.Count == 0)
        {
            if (other.gameObject.layer == 10)
                Rotate(other.gameObject);
        }
        else
        {
            bool isTakenRoad = false;
            foreach (GameObject r in roads)
            {
                if(r == other.gameObject)
                {
                    isTakenRoad = true;
                    break;
                }
            }
            if (!isTakenRoad && other.gameObject.layer == 10)
                    Rotate(other.gameObject);
        }
    }
}

