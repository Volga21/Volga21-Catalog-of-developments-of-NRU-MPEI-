using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceLogicOnePlane : MonoBehaviour
{
    public Button button;
    //public Spawner spawner;
    public Material[] mat;
    List<Material> oldMat = new List<Material>();
    bool active = false;

    OnePlaneCuttingController c;

    void Start()
    {
        button = this.GetComponent<Button>();
       // button.onClick.AddListener(ActiveSlice);
    }


    //void ActiveSlice()
    //{
    //    //c = handler.objModel.gameObject.GetComponent<OnePlaneCuttingController>();
    //    //if (!c)
    //    //{
    //    //    handler.objModel.gameObject.AddComponent(typeof(OnePlaneCuttingController));
    //    //    c = handler.objModel.gameObject.GetComponent<OnePlaneCuttingController>();
    //    //}

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
    //        //part.gameObject.GetComponent<OnePlaneCuttingController>().plane = handler.Plane;
    //        //part.gameObject.GetComponent<OnePlaneCuttingController>().flag = true;
    //        //part.gameObject.GetComponent<MeshRenderer>().materials = mat;
    //        //oldMat.Add(part.gameObject.GetComponent<MeshRenderer>().material);
    //        part.gameObject.GetComponent<OnePlaneCuttingController>().mat = mat;
    //        part.gameObject.GetComponent<OnePlaneCuttingController>().SetCutMat();
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
    //        part.gameObject.GetComponent<OnePlaneCuttingController>().SetOldMat();
    //    }
    //    handler.Plane.GetComponent<CutPlane>().RefreshMat();
    //}
}
