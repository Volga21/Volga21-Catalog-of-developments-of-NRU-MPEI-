using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private Transform _rotationToCamera;

    private void Start()
    {
        _rotationToCamera = Camera.main.transform;
        _canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        _canvas.transform.LookAt(_rotationToCamera);
    }
}
