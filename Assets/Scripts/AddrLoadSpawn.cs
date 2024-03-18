using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddrLoadSpawn : MonoBehaviour
{
    public static AddrLoadSpawn Instance { get; private set; }
    public List<GameObject> _mMyGameObjectL;
    public List<AssetReferenceGameObject> refGObjectsList;
    public List<AssetLabelReference> labelRefs;
    public AsyncOperationHandle _asyncOperationHandle;
    public Button btnCollection;

    public GameObject loadingCirc;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        //PlayerPrefs.HasKey("labelRefs");
        //if (!PlayerPrefs.HasKey("labelRefs"))
        //{
        //    PlayerPrefs.SetString("labelRefs", "");
        //}
        //labelRefs = PlayerPrefs.GetString("labelRefs", "").Split(" ").ToList();

        //Addressables.CleanBundleCache();
        //foreach (AssetLabelReference label in labelRefs)
        //    Addressables.ClearDependencyCacheAsync(label);
        //Addressables.ClearResourceLocators();
        //Caching.ClearCache();


        //foreach (AssetLabelReference label in labelRefs)
        //{
        //    LoadLabelRef(label);
        //    Debug.Log(label.RuntimeKey);
        //}
        //_asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(labelRef);
    }

    public async void LoadLabelRef(AssetLabelReference label)
    {
        Debug.Log(label);
        btnCollection.enabled = false;
        _asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(label);
        loadingCirc.gameObject.SetActive(true);
        await _asyncOperationHandle.Task;
        loadingCirc.gameObject.SetActive(false);
        btnCollection.enabled = true;
    }

    public async void LoadLabelRef(string keyRef,int index)
    {
        Debug.Log(labelRefs[index]);
        btnCollection.enabled = false;
        _asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(labelRefs[index]);
        loadingCirc.gameObject.SetActive(true);
        await _asyncOperationHandle.Task;
        if(_asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            PlayerPrefs.SetInt(keyRef, 1);
        }
        loadingCirc.gameObject.SetActive(false);
        btnCollection.enabled = true;
    }
    private void Start()
    {
        _mMyGameObjectL = new List<GameObject>();
    }
    public void InstantiateGameobjectUsingAssetReference(string key)
    {
        Addressables.InstantiateAsync(key, this.transform).Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        _mMyGameObjectL.Add(obj.Result);
    }

    public void ReleaseGameobjectUsingAssetReference()
    {
        while (_mMyGameObjectL.Count > 0)
        {
            Destroy(_mMyGameObjectL[0]);
            //Addressables.Release(_mMyGameObjectL[0]);
            _mMyGameObjectL.Remove(_mMyGameObjectL[0]);
        }
    }
}
