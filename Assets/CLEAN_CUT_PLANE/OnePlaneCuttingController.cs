using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//[ExecuteInEditMode]
public class OnePlaneCuttingController : MonoBehaviour {

    public GameObject plane;
    public Material[] mat;
    public Vector3 normal;
    public Vector3 position;
    public Renderer rend;
    List<Material> m = new List<Material>();

    public bool flag = false;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        if (rend)
        {
            foreach (Material item in rend.materials)
            {
                m.Add(item);
            }
        }
    }
    //void FixedUpdate ()
    //{
    //    if (flag && rend)
    //    {
    //        UpdateShaderProperties();
    //    }
    //}

    //private void UpdateShaderProperties()
    //{

    //    normal = plane.transform.TransformVector(new Vector3(0, 0, -1));
    //    position = plane.transform.position;
    //    for(int i=0;i<rend.materials.Length;i++)
    //    {
    //        if(rend.materials[i].shader.name== "CrossSection/OnePlaneBSP")
    //        {
    //            rend.materials[i].SetVector("_PlaneNormal", normal);
    //            rend.materials[i].SetVector("_PlanePosition", position);
    //        }
    //    }
        
    //}
    public void SetOldMat()
    {
        if (rend)
        {
            rend.materials = m.ToArray();
        }
    }
    public void SetCutMat()
    {
        if (rend)
        {
            rend.materials = mat;
        }
    }
}
