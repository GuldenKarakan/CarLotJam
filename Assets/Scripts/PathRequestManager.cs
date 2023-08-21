using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Yol isteklerini yöneten sýnýf
public class PathRequestManager : MonoBehaviour
{
    [HideInInspector] public PathFinding pathFinding; // Yol bulma iþlemlerini gerçekleþtiren sýnýf
    // Yol isteklerinin sýrasýný tutan kuyruk
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;// Þu anki yol isteði

    public static PathRequestManager instance; // Yol istek yöneticisi örneði

    bool isProcessingPath; // Bir yol iþleme sürecinin devam edip etmediðini belirten flag

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>(); // Yol bulma bileþenini al
    }

    // Dýþarýdan yol isteði yapýlabilen metot
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        // Yeni bir yol isteði oluþturulur
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        // Yol isteði kuyruða eklenir
        instance.pathRequestQueue.Enqueue(newRequest);
        // Þu anki iþlemi deneyerek bir sonraki yol isteðini iþlemeye çalýþ
        instance.TryProcessNext();
    }

    // Bir sonraki yol isteðini iþlemeye çalýþan metot
    void TryProcessNext()
    {
        // Eðer þu an bir yol iþlemesi yoksa ve yol isteði kuyruðu boþ deðilse
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            // Þu anki yol isteðini al
            currentPathRequest = pathRequestQueue.Dequeue();
            // Yol iþlemesi devam ediyor olarak iþaretle
            isProcessingPath = true;
            // Yol bulma iþlemine baþla
            pathFinding.StartFinfPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // Yol iþleme tamamlandýðýnda çaðrýlan metot
    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        // Bir sonraki yol isteðini iþlemeye çalýþ
        TryProcessNext();
    }
    // Yol isteði yapýsý
    struct PathRequest
    {
        public Vector3 pathStart; // Yolun baþlangýç noktasý
        public Vector3 pathEnd; // Yolun hedef noktasý
        public Action<Vector3[], bool> callback; // Yol hesaplamasý tamamlandýðýnda çaðrýlacak fonksiyon

        // Constructor
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _calback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _calback;
        }
    }
}

