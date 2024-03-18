using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlicePlane : MonoBehaviour
{
    public GameObject plane;
    public float startPosition;
    public float length;
    public bool XZ;
    public bool YZ;
    public bool XY;
    Slider s;

    private void Start()
    {
        s = gameObject.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        if (XZ)
        {
            plane.transform.position = new Vector3(0, startPosition - s.value * length, 0);
        }
        if (YZ)
        {
            plane.transform.position = new Vector3(startPosition - s.value * length, 0, 0);
        }
        if (XY)
        {
            plane.transform.position = new Vector3(0, 0, startPosition - s.value * length);
        }
        
    }

    //public void SetPosition(float p)
    //{
    //    position = p;
    //}
}
