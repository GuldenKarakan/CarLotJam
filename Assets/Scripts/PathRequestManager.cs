using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Yol isteklerini y�neten s�n�f
public class PathRequestManager : MonoBehaviour
{
    [HideInInspector] public PathFinding pathFinding; // Yol bulma i�lemlerini ger�ekle�tiren s�n�f
    // Yol isteklerinin s�ras�n� tutan kuyruk
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;// �u anki yol iste�i

    public static PathRequestManager instance; // Yol istek y�neticisi �rne�i

    bool isProcessingPath; // Bir yol i�leme s�recinin devam edip etmedi�ini belirten flag

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>(); // Yol bulma bile�enini al
    }

    // D��ar�dan yol iste�i yap�labilen metot
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        // Yeni bir yol iste�i olu�turulur
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        // Yol iste�i kuyru�a eklenir
        instance.pathRequestQueue.Enqueue(newRequest);
        // �u anki i�lemi deneyerek bir sonraki yol iste�ini i�lemeye �al��
        instance.TryProcessNext();
    }

    // Bir sonraki yol iste�ini i�lemeye �al��an metot
    void TryProcessNext()
    {
        // E�er �u an bir yol i�lemesi yoksa ve yol iste�i kuyru�u bo� de�ilse
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            // �u anki yol iste�ini al
            currentPathRequest = pathRequestQueue.Dequeue();
            // Yol i�lemesi devam ediyor olarak i�aretle
            isProcessingPath = true;
            // Yol bulma i�lemine ba�la
            pathFinding.StartFinfPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // Yol i�leme tamamland���nda �a�r�lan metot
    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        // Bir sonraki yol iste�ini i�lemeye �al��
        TryProcessNext();
    }
    // Yol iste�i yap�s�
    struct PathRequest
    {
        public Vector3 pathStart; // Yolun ba�lang�� noktas�
        public Vector3 pathEnd; // Yolun hedef noktas�
        public Action<Vector3[], bool> callback; // Yol hesaplamas� tamamland���nda �a�r�lacak fonksiyon

        // Constructor
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _calback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _calback;
        }
    }
}

