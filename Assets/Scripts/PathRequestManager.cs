using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Yol isteklerini y�neten s�n�f
public class PathRequestManager : MonoBehaviour
{
    [HideInInspector] public PathFinding pathFinding;
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PathRequestManager instance;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    // D��ar�dan yol iste�i yap�labilen metot
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    // Bir sonraki yol iste�ini i�lemeye �al��an metot
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFinfPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // Yol i�leme tamamland���nda �a�r�lan metot
    public void FinishProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _calback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _calback;
        }
    }
}

