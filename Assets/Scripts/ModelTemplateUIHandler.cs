using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ModelTemplateUIHandler : MonoBehaviour
{
    [SerializeField]
    //public ObjectModel objModel;
    public TMP_Text modelName;
    public GameObject modelNameBg;

    //public CollectionUIHandler CollectionUI;
    //public GameObject MediaContent;
    //public GameObject ToMMenu;

    public GameObject shortDescrUI;
    public GameObject sliceUI;
    public GameObject splitUI;
    public GameObject videoUI;
    public GameObject docUI;
    public GameObject wwwUI;

    public GameObject Plane;
    public GameObject PlaneXZ;
    public GameObject PlaneYZ;
    public GameObject PlaneXY;
    public DocsHandler docsHandler;
    public static ModelTemplateUIHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
           Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateUIModelTemplate()
    {
        //if (objModel.canSlice)
        //{
        //    Plane.GetComponent<CutPlane>().RefreshClice();
        //}
        modelName.text = Spawner.Instance.objectModel.modelName;
        UpdateUi();
        Debug.Log("UpdateUIModelTemplate");
    }
    public void UpdateUi()
    {
        shortDescrUI.SetActive(Spawner.Instance.objectModel.hasShortDescr);
        sliceUI.SetActive(Spawner.Instance.objectModel.canSlice);
        splitUI.SetActive(Spawner.Instance.objectModel.hasAnimation);
        videoUI.SetActive(Spawner.Instance.objectModel.hasVideo);
        docUI.SetActive(Spawner.Instance.objectModel.hasDoc);
        wwwUI.SetActive(Spawner.Instance.objectModel.hasLink);
    }
}
