using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSliceBtnScript : MonoBehaviour
{
    public ModelTemplateUIHandler handler;
    public bool setActive = true;
    public List<GameObject> ObjectHide;
    public List<GameObject> ObjectDisplay;

    public virtual void Active()
    {
        DisplayHide();
        if (setActive)
        {
            handler.UpdateUi();
        }
    }

    protected void DisplayHide()
    {
        foreach (GameObject item in ObjectDisplay)
        {
            item.SetActive(setActive);
        }
        setActive = !setActive;
        foreach (GameObject item in ObjectHide)
        {
            item.SetActive(setActive);
        }
    }
}
