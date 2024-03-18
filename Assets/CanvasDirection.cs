using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDirection : MonoBehaviour
{
    public Transform cam;
    // Update is called once per frame
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
