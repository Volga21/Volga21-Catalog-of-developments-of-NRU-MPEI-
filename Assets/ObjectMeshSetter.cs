using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectMeshSetter : MonoBehaviour
{
    //public List<MeshRenderer> meshList = new List<MeshRenderer>();
    public Material mat;
    public Transform thistr;
    // Start is called before the first frame update
    [ContextMenu("set material!")]
    public void collect()
    {
        foreach (Transform item in thistr)
        {
            try
            {
                MeshRenderer mr = item.gameObject.GetComponent<MeshRenderer>();
                mr.materials = Enumerable.Repeat(mat, mr.materials.Length).ToArray();
                //for (int i = 0; i < mr.materials.Length; i++)
                //{
                //    mr.materials[i] = mat;
                //}
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}
