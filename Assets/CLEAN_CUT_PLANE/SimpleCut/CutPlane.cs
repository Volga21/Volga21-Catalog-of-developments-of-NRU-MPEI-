using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPlane : MonoBehaviour {

    private Vector3 normal;
    public GameObject plane;



    public List<GameObject> clipObj = new List<GameObject>();
    public List<Material> clipObjMat = new List<Material>();

    void Start () {
        normal = this.GetComponent<MeshFilter>().mesh.normals[0];
       // RefreshClice();
    }
    void Update () {
        foreach (var item in clipObjMat)
        {

            item.SetVector("_PlaneCenter", this.transform.position);
            item.SetVector("_PlaneNormal", this.transform.TransformDirection(this.normal));

            //normal = plane.transform.TransformVector(new Vector3(0, 0, -1));
            //item.SetVector("_PlaneNormal", normal);
            //item.SetVector("_PlanePosition", plane.transform.position);
        }
    }

    public void RefreshClice()
    {
        clipObj = new List<GameObject>() { Spawner.Instance.objectModel.gameObject };
        RefreshMat();
    }

    public void RefreshMat()
    {
        clipObjMat = new List<Material>();
        if (clipObj[0].gameObject.GetComponent<MeshRenderer>())
        {
            clipObjMat.Add(clipObj[0].gameObject.GetComponent<MeshRenderer>().material);
        }
        foreach (Transform item in clipObj[0].transform)
        {
            if (item.gameObject.GetComponent<MeshRenderer>())
            {
                clipObjMat.Add(item.gameObject.GetComponent<MeshRenderer>().material);
            }
            
        }
    }
}
