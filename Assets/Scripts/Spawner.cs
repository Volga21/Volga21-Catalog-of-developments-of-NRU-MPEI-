using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class Spawner : MonoBehaviour
{
    [Header("asd")]
    public CollectionUIHandler collectionUIHandler;
    [Header("Model classes list")]

    [SerializeField]
    private CameraTarget cameraTarget;
    public ObjectModel objectModel;
    public GameObject lookAt;

    public static Spawner Instance { get; private set; }
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
    //private void Awake()
    //{
    //    try
    //    {
    //        foreach (AssetBundle bundl in LoadScript.instance.assetBundle)
    //        {
    //            GameObject[] db = bundl.LoadAllAssets<GameObject>();
    //            Debug.Log(db.Length);
    //            foreach (var s in bundl.GetAllAssetNames())
    //            {
    //                Debug.Log(s);
    //            }


    //            foreach (var item in db)
    //            {
    //                if (item.GetComponent<HydrogenEnergyModel>() != null)
    //                {
    //                    collectionUIHandler.mainTypesList[0].modelPrefabs[0].sampleList.Add(item.GetComponent<HydrogenEnergyModel>());
    //                }
    //                if (item.GetComponent<EnergFieldClimTransModel>() != null)
    //                    collectionUIHandler.mainTypesList[1].modelPrefabs[0].sampleList.Add(item.GetComponent<EnergFieldClimTransModel>());
    //                if (item.GetComponent<DistrRenewEnergyModel>() != null)
    //                    collectionUIHandler.mainTypesList[2].modelPrefabs[0].sampleList.Add(item.GetComponent<DistrRenewEnergyModel>());
    //                if (item.GetComponent<DigitEnergyModel>() != null)
    //                    collectionUIHandler.mainTypesList[3].modelPrefabs[0].sampleList.Add(item.GetComponent<DigitEnergyModel>());
    //                if (item.GetComponent<nghceTermalPowerModel>() != null)
    //                    collectionUIHandler.mainTypesList[4].modelPrefabs[0].sampleList.Add(item.GetComponent<nghceTermalPowerModel>());
    //                if (item.GetComponent<nghceHydroPowerModel>() != null)
    //                    collectionUIHandler.mainTypesList[4].modelPrefabs[1].sampleList.Add(item.GetComponent<nghceHydroPowerModel>());
    //            }
    //        }

    //    }
    //    catch (System.Exception e)
    //    {
    //        Debug.Log("START NOT FROM MAIN MENU");
    //        Debug.Log(e.ToString());
    //    }
    //}

    public void SpawnObject(ObjectModel _objectModel)
    {
        foreach (Transform child in transform)
        {
            //GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
        objectModel = _objectModel;
        objectModel.gameObject.SetActive(true);
        // Instantiate(om, transform);
        ModelTemplateUIHandler.Instance.UpdateUIModelTemplate();
        ModelTemplateUIHandler.Instance.modelNameBg.SetActive(true);
        cameraTarget.ChangeTarget();
    }
    public void ClearObject(){
        foreach (Transform child in transform)
        {
            //GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
    public void SplitToParts()
    {
        objectModel.Decomposition();
    }
}
