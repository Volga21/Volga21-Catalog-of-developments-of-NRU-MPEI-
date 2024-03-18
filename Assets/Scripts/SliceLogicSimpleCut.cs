using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceLogicSimpleCut : MonoBehaviour
{
    public Button button;
    public Material[] mat;
    bool active = false;

    void Start()
    {
        button = this.GetComponent<Button>();
        //button.onClick.AddListener(ActiveSlice);
    }


    //void ActiveSlice()
    //{
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
    //    //c.plane = handler.Plane;
    //    //c.flag = true;
    //    foreach (Transform part in handler.objModel.transform)
    //    {
    //        //Debug.Log(part.name);
    //        //part.gameObject.GetComponent<OnePlaneCuttingController>().plane = handler.Plane;
    //        //part.gameObject.GetComponent<OnePlaneCuttingController>().flag = true;
    //        //part.gameObject.GetComponent<MeshRenderer>().materials = mat;
    //        part.gameObject.GetComponent<SimpleCutDetail>().mat = mat;
    //        part.gameObject.GetComponent<SimpleCutDetail>().SetCutMat();
            
    //        //oldMat.Add(part.gameObject.GetComponent<MeshRenderer>().materials);
    //    }
    //    if (handler.objModel.GetComponent<MeshRenderer>())
    //    {
    //        handler.objModel.GetComponent<MeshRenderer>().material = mat[0];
    //    }
    //    handler.Plane.GetComponent<CutPlane>().RefreshMat();
    //    //Debug.Log("activated");
    //}

    //public void Deactivated()
    //{
    //    active = false;
    //    //Debug.Log("diactivated(((((((");
    //    if (handler.objModel.GetComponent<MeshRenderer>())
    //    {
    //        handler.objModel.GetComponent<MeshRenderer>().material = handler.objModel.mat;
    //    }
    //    foreach (Transform part in handler.objModel.transform)
    //    {
    //        part.gameObject.GetComponent<SimpleCutDetail>().SetOldMat();
    //    }
    //    handler.Plane.GetComponent<CutPlane>().RefreshMat();
    //}
}