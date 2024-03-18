using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceModel : ObjectModel
{
    override protected void Awake()
    {
        base.Awake();
        //foreach (Transform item in this.transform)
        //{
        //    if (item.GetComponent<MovablePart>())
        //    {
        //        parts.Add(item.GetComponent<MovablePart>());
        //    }
            
        //    if (canSlice)
        //    {
        //        item.gameObject.AddComponent(typeof(SimpleCutDetail));
        //        // для разных механик разреза
        //        //item.gameObject.AddComponent(typeof(ThreeAAPlanesCuttingController));
        //        //item.gameObject.AddComponent(typeof(OnePlaneCuttingController));
        //    }
        //}
    }
}
