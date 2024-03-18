using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public bool activate = false;

    void Update()
    {
        if (Input.GetMouseButton(0) && activate)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.gameObject.GetComponent<ObjectModel>() != null)
                    {
                        Debug.Log(hit.transform.name);
                    }
                    
                }
            }
        }
    }
}
