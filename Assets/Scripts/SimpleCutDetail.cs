using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCutDetail : MonoBehaviour
{
    public Material[] mat;
    public MeshRenderer rend;
    List<Material> m = new List<Material>();

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
