using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceLogicThreePlanes : MonoBehaviour
{
    public Button button;
    //public Spawner spawner;
    public Material[] mat;
    List<Material> oldMat = new List<Material>();
    ObjectModel objectModel;
    bool active = false;

    ThreeAAPlanesCuttingController c;

    void Start()
    {
        button = this.GetComponent<Button>();
        //button.onClick.AddListener(ActiveSlice);
    }


    //void ActiveSlice()
    //{
    //    c = handler.objModel.gameObject.GetComponent<ThreeAAPlanesCuttingController>();
    //    if (!c)
    //    {
    //        handler.objModel.gameObject.AddComponent(typeof(ThreeAAPlanesCuttingController));
    //        c = handler.objModel.gameObject.GetComponent<ThreeAAPlanesCuttingController>();
    //    }

    //    if (!active)
    //    {
    //        Activated();
    //    }
    //    else
    //    {
    //        Deactivated();
    //    }
    //}

    //public void Activated()
    //{
    //    active = true;
    //    c.planeXY = handler.PlaneXY;
    //    c.planeXZ = handler.PlaneXZ;
    //    c.planeYZ = handler.PlaneYZ;
    //    c.flag = true;
    //    foreach (Transform part in handler.objModel.transform)
    //    {
    //        //Set to each piece planes and material, save old material
    //        part.gameObject.GetComponent<ThreeAAPlanesCuttingController>().planeXY = handler.PlaneXY;
    //        part.gameObject.GetComponent<ThreeAAPlanesCuttingController>().planeXZ = handler.PlaneXZ;
    //        part.gameObject.GetComponent<ThreeAAPlanesCuttingController>().planeYZ = handler.PlaneYZ;
    //        part.gameObject.GetComponent<ThreeAAPlanesCuttingController>().flag = true;
    //        part.gameObject.GetComponent<MeshRenderer>().materials = mat;
    //        oldMat.Add(part.gameObject.GetComponent<MeshRenderer>().material);
            
    //    }
    //    if (handler.objModel.GetComponent<MeshRenderer>())
    //    {
    //        handler.objModel.GetComponent<MeshRenderer>().material = mat[0];
    //    }

    //    //Debug.Log("activated");
    //}

    //public void Deactivated()
    //{
    //    active = false;
    //    //Debug.Log("diactivated(((((((");
    //    if (handler.objModel.GetComponent<MeshRenderer>())
    //    {
    //        handler.objModel.GetComponent<MeshRenderer>().material = (handler.objModel as ObjectModel).mat;
    //    }
    //    foreach (Transform part in handler.objModel.transform)
    //    {
    //        part.gameObject.GetComponent<ThreeAAPlanesCuttingController>().SetOldMat();
    //    }
    //}
}
