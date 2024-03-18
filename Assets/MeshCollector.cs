using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCollector : MonoBehaviour
{
    public List<MeshRenderer> meshList = new List<MeshRenderer>();
    // Start is called before the first frame update
    [ContextMenu("COLLECT!")]
    public void collect()
    {
        foreach (var item in GameObject.FindObjectsOfType(typeof(MeshRenderer)))
        {
            meshList.Add((MeshRenderer)item);
        }
       
    }
}
