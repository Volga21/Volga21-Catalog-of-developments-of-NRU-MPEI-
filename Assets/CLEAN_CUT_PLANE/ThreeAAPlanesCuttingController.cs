using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeAAPlanesCuttingController : MonoBehaviour {

    public GameObject planeYZ;
    public GameObject planeXZ;
    public GameObject planeXY;
    Material mat;
    public Vector3 positionYZ;
    public Vector3 positionXZ;
    public Vector3 positionXY;
    public Renderer rend;
    List<Material> m = new List<Material>();

    public bool flag = false;
    // Use this for initialization
    void Start()
    {

        rend = GetComponent<Renderer>();
        if (rend)
        {
            foreach (Material item in rend.materials)
            {
                m.Add(item);
            }
        }
        //UpdateShaderProperties();
    }

    void FixedUpdate()
    {
        if (flag && rend)
        {
            UpdateShaderProperties();
        }
    }

    private void UpdateShaderProperties()
    {
        positionYZ = planeYZ.transform.position;
        positionXZ = planeXZ.transform.position;
        positionXY = planeXY.transform.position;
        for (int i = 0; i < rend.materials.Length; i++)
        {
            if (rend.materials[i].shader.name == "CrossSection/ThreeAAPlanesBSP")
            {
                rend.materials[i].SetVector("_Plane1Position", positionYZ);
                rend.materials[i].SetVector("_Plane2Position", positionXZ);
                rend.materials[i].SetVector("_Plane3Position", positionXY);
            }
        }
      
    }
   
    public void SetOldMat()
    {
        if (rend)
        {
            rend.materials = m.ToArray();
        }
    }
}
