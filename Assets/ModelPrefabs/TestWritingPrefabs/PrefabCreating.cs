using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PrefabCreating : MonoBehaviour
{
    public GameObject prefab = null;
    public string prefName = "";

    //[ContextMenu("CREATE PREFAB!")]
    //public void Prefab()
    //{
    //    this.AddComponent<NewGenHighCapEnergyModel>();
    //    PrefabUtility.SaveAsPrefabAsset(this.gameObject, "Assets/ModelPrefabs/TestWritingPrefabs/"+ prefName + ".prefab", out bool a);
    //    if (a)
    //    {
    //        Debug.Log("OK!");
    //    }
    //}

}
