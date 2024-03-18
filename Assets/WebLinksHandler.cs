using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebLinksHandler : MonoBehaviour
{
    public GameObject contentParent;
    public GameObject linkHolderPref;
    public LinkHandler linkHandler;
    //public TMP_Text loadProcent;
    //public Button button;
    ObjectModel objectModel;
    string link;
    public void RefreshLinks()
    {
        foreach (Transform item in contentParent.transform)
        {
            Destroy(item.gameObject);
        }
        objectModel = Spawner.Instance.objectModel;
        for (int i = 0; i < objectModel.URLNames.Count; i++)
        {
            linkHandler = Instantiate(linkHolderPref,contentParent.transform).GetComponent<LinkHandler>();

            linkHandler.Init(objectModel.URLNames[i], objectModel.URLLinks[i]);
        }
    }
}
