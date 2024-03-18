using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;
using System.IO;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.ResourceManagement;
    public class JsPrefabs
    {   
        public string Name;
        public int Version;
        public double Size;
        public int SubVersion;
    }

public class MainTypesListTemplate : MonoBehaviour
{
    Button btn;
    [SerializeField] public GameObject Canvas;
    [SerializeField] public GameObject TogglePrefab; 
    public string keyRef;
    public CollectionUIHandler collectionUIHandler;
    public TMP_Text typeNameTxt;
    public int typeId;
    public string typeName;
    public List<string> subTypes = new List<string>();
    public List<JsPrefabs> updatePrefabs = new List<JsPrefabs>();
    public List<JsPrefabs> hasPrefabs=new List<JsPrefabs>();

    public string s;

    public bool setupIsDone = false;
    public AssetReferenceGameObject assetReference;

    [System.Serializable]
    public class serializableObjectModel
    {
        public List<ObjectModel> sampleList;
    }
    public List< serializableObjectModel> objectModelPrefabs = new List<serializableObjectModel>();

    [System.Serializable]
    public class serializableAdressableKeys
    {
        public List<GameObject> sampleList;
    }
    public List<serializableAdressableKeys> objectModelKeys = new List<serializableAdressableKeys>();
    public List<serializableAdressableKeys> objectModelTemp = new List<serializableAdressableKeys>();
    public bool internetConnection=true;
    public string path;
    private IDTransformer urladres;
    public string error="Не удалось загрузить: ";
    public string  catalog="";
    public bool loadJs=false;
    public async UniTaskVoid Setup()
    {

        UniTask task = TypeTemplateSetup();
        await task;
        setupIsDone = true;
        Debug.Log("setupIsDone");
    }
    void Awake()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
            path=Path.Combine(Application.persistentDataPath,$"Has{s}.json");
        #else
            path=Path.Combine(Application.dataPath,$"Has{s}.json");
        #endif
        if(!File.Exists(path)){
            string json = JsonConvert.SerializeObject(hasPrefabs);
            File.WriteAllText(path, json);
        }
        btn = GetComponent<Button>();
        string layout = System.IO.File.ReadAllText(path);
        hasPrefabs=JsonConvert.DeserializeObject<List<JsPrefabs>>(layout);
        StartCoroutine(LoadJson());
        for(int i=0;i<hasPrefabs.Count;i++){   
            StartCoroutine(startInstall(hasPrefabs[i].Name,hasPrefabs[i].SubVersion,hasPrefabs[i]));
        }
        if(hasPrefabs.Count==0){
            setupIsDone=true;
        }
        //btn.onClick.AddListener(SelectType);
        btn.onClick.AddListener(SelectType);
    }
    IEnumerator LoadJson(){
        UnityWebRequest request=UnityWebRequest.Get("http://80.87.201.230/download/");
        yield return request.SendWebRequest();
        if(request.isNetworkError==true){
            internetConnection=false;
            loadJs=true;
        }
        else{
            AsyncOperationHandle<TextAsset> handle;
            handle=Addressables.LoadAssetAsync<TextAsset>(s);
            while(!handle.IsDone){
                yield return handle;
            }
            if(handle.Status==AsyncOperationStatus.Succeeded){
                Debug.Log("loadJson");
                string json=handle.Result.text;
                Debug.Log(handle.Result.text);
                updatePrefabs= JsonConvert.DeserializeObject<List<JsPrefabs>>(json);
                loadJs=true;
            }
            else{
                catalog="каталог обновления "+typeName;
                loadJs=true;
            }
        }
    }
    IEnumerator startInstall(string name,int subV,JsPrefabs jsname){
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
         while(!handle.IsDone){
            yield return null;
        }
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            objectModelKeys[subV].sampleList.Add(handle.Result);
            var handle2 = Addressables.InstantiateAsync(name, Spawner.Instance.transform);
            yield return handle2;
            if(handle2.Status==AsyncOperationStatus.Succeeded){
                GameObject _cache = handle2.Result;
                objectModelTemp[subV].sampleList.Add(_cache);
                if (_cache.TryGetComponent(out ObjectModel component) == false)
                    throw new NullReferenceException($"Object of type {typeof(ObjectModel)} is null on " +"attempt to load it from addressables");
                objectModelPrefabs[subV].sampleList.Add(component);
            }
        }
        else{
            if(error.Length!=0){
                error+=", "+name;
            }
            else{
                error+=name;
            }
            
            hasPrefabs.Remove(jsname);
        }
        int sum=0;
        for(int i=0;i<objectModelPrefabs.Count;i++){
            sum=sum+objectModelPrefabs[i].sampleList.Count;
        }
        //можно переделать
        while(sum!=hasPrefabs.Count || !loadJs){
            sum=0;
            for(int i=0;i<objectModelPrefabs.Count;i++){
                sum=sum+(objectModelPrefabs[i].sampleList.Count);
            }   
            yield return null;
        }
        //можно переделать
        setupIsDone=true;
    }
    async UniTask TypeTemplateSetup()
    {
        for(int i=0;i<objectModelPrefabs.Count;i++){
            objectModelPrefabs[i].sampleList.Clear();
        }
        Debug.Log($"{objectModelKeys.Count} - размер превого элемента массива");
        for (int i = 0; i < objectModelKeys.Count; i++)
        {
            //objectModelPrefabs.Add(new serializableObjectModel());
            //objectModelPrefabs[i] = new serializableObjectModel();
            //objectModelPrefabs[i].sampleList = new List<ObjectModel>();
            
            for (int j = 0; j < objectModelKeys[i].sampleList.Count; j++)
            {   
                Debug.Log($"{objectModelKeys[i].sampleList.Count} - размер подмассива");
                //if (assetReferenceGameObject.Asset)
                {
                   // Debug.Log("loaded already");
                    var model = await LoadObjectModel(hasPrefabs[j].Name);
                   // model.gameObject.SetActive(false);
                    objectModelPrefabs[i].sampleList.Add(model);
                }
                //else { Debug.Log("not loaded "); }
                
                
            }
        }
    }

    //async UniTaskVoid Start()
    //{
    //    btn = GetComponent<Button>();
    //    btn.onClick.AddListener(SelectType);
    //    for (int i = 0; i < objectModelKeys.Count; i++)
    //    {
    //        objectModelPrefabs.Add(new serializableObjectModel());
    //        objectModelPrefabs[i] = new serializableObjectModel();
    //        objectModelPrefabs[i].sampleList = new List<ObjectModel>();
    //        for (int j = 0; j < objectModelKeys[i].sampleList.Count; j++)
    //        {
    //            var model = await LoadObjectModel(objectModelKeys[i].sampleList[j]);
    //            objectModelPrefabs[i].sampleList.Add(model);
    //            model.gameObject.SetActive(false);
    //        }
    //    }
    //}

    private async UniTask<ObjectModel> LoadObjectModel(string s)
    {
        //Debug.Log(asset);
        var handle = Addressables.InstantiateAsync(s, Spawner.Instance.transform);

        //Debug.Log(handle);
        GameObject _cache = await handle.Task;
        if(_cache==null ){
            Debug.Log("_cache is null");
        }
        Debug.Log("i was here");

        if (_cache.TryGetComponent(out ObjectModel component) == false)
            throw new NullReferenceException($"Object of type {typeof(ObjectModel)} is null on " +
                                             "attempt to load it from addressables");
        //Debug.Log(component);
        return component;
    }

    void SelectType()
    {
        collectionUIHandler.FillTypeContent(this);
    }
}
