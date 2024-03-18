using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.IO;
public class BundleData
{
    public int id { get; set; }
    public uint version { get; set; }
    public string bundleName { get; set; }
}
public class GameLoader : MonoBehaviour
{
    public GameObject load_manager;
    public GameObject verP;
    public GameObject netwServPanel;
    public TMP_Text netwServPanelText;

    public TMP_Text cache;
    string cachestr;
    string cacheFolder = "cacheFolder";
    string cacheFolderPath;

    //google https://drive.google.com/uc?export=download&id=1kgjtVneiR0YAE_7o-yby00u-vPq05phY
    private string rootUrl = "https://drive.google.com/";
    //server
    //private string rootUrl = "http://127.0.0.1:8000/";

    public List<AssetBundle> assetBundle = new List<AssetBundle>();

    private List<BundleData> bundleData = new List<BundleData>();
    private LoadScript lm;

    void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        if (!PlayerPrefs.HasKey("version"))
        {
            PlayerPrefs.SetInt("version", 0);
        }
        // Инициализация игровой базы
        if (LoadScript.instance == null)
        {
            load_manager = Instantiate(load_manager);
        }
        lm = load_manager.GetComponent<LoadScript>();
        lm.gameLoader = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        cacheFolderPath = Path.Combine(Caching.currentCacheForWriting.path, cacheFolder);
#endif
#if UNITY_EDITOR_WIN
        cacheFolderPath = Path.Combine(@"C:\Users\KanD\AppData\LocalLow\Unity\DefaultCompany_MPEI_Innovations", cacheFolder);
        Debug.Log(cacheFolderPath);
#endif
        // можно просто убарать, печатает пути к разным данным приложения
        cachestr = Application.persistentDataPath + "\n";
        cachestr += Application.dataPath + "\n";
        cachestr += Application.temporaryCachePath + "\n";
        cachestr += Application.streamingAssetsPath + "\n";
        cachestr += Caching.currentCacheForWriting.path.ToString();
        cache.text = cachestr;

        //Caching.ClearCache(20);
        //StartCoroutine(GetLocalCache());
        RefreshNetData();
    }
    public void RefreshNetData()
    {
        StartCoroutine(checkConnection("http://google.com", (isConnected) =>
        {
            if (isConnected)
            {
                Debug.Log("network connected");
                netwServPanel.SetActive(false);
                StartCoroutine(checkConnection(rootUrl, (isConnected) =>
                {
                    if (isConnected)
                    {
                        Debug.Log("server connected");
                        netwServPanel.SetActive(false);
                        StartCoroutine(GetData());
                    }
                    else
                    {
                        Debug.Log("server not connected");
                        netwServPanelText.text = "Нет подключения к серверу\nНажать для обновления";
                        netwServPanel.SetActive(true);
                    }
                }));
            }
            else
            {
                Debug.Log("network not connected");
                netwServPanelText.text = "Нет подключения к интернету\nНажать для обновления";
                netwServPanel.SetActive(true);

                //StartCoroutine(GetLocalCache());

            }
        }));
    }
    public void setChoisse(string s)
    {
        foreach (BundleData bundD in bundleData)
        {
            StartCoroutine(LoadData(s, bundD));
        }
    }

    IEnumerator GetData()
    {
        bool flagLoadCache = false;
        List<string> bundleNames = new List<string>();
        //server
        //UnityWebRequest www = new UnityWebRequest(rootUrl + "data/");
        //google
        UnityWebRequest www = new UnityWebRequest(rootUrl + "uc?export=download&id=1kgjtVneiR0YAE_7o-yby00u-vPq05phY");

        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            GetBundleData(www.downloadHandler.text);

            foreach (BundleData bundD in bundleData)
            {
                Debug.Log(bundD.bundleName);
                if (!PlayerPrefs.HasKey(bundD.bundleName + "_version"))
                {
                    PlayerPrefs.SetInt(bundD.bundleName + "_version", 0);
                    bundleNames.Add(bundD.bundleName);
                    //verP.SetActive(true);
                }
                else if (bundD.version > PlayerPrefs.GetInt(bundD.bundleName + "_version"))
                {
                    bundleNames.Add(bundD.bundleName);
                    //verP.SetActive(true);
                }
                else
                {
                    flagLoadCache = true;
                    

                    //StartCoroutine(LoadData("old", bundD));
                }
            }
            if (flagLoadCache)
            {
                StartCoroutine(GetLocalCache());
            }
            Debug.Log("bundleNames.Count" + bundleNames.Count);
            if (bundleNames.Count > 0)
            {
                verP.SetActive(true);
            }

        }
    }
    IEnumerator GetLocalCache()
    {
        DirectoryInfo di = new DirectoryInfo(cacheFolderPath);
        Debug.Log("directory FullName ::: " + di.FullName);
        // Get a reference to each directory in that directory.
        DirectoryInfo[] diArr = di.GetDirectories();

        // Display the names of the directories.
        foreach (DirectoryInfo dri in diArr)
        {
            Debug.Log("directory name ::: " + dri.FullName);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(cacheFolderPath, dri.Name, "__data"));
            yield return bundleLoadRequest;

            AssetBundle bundle = bundleLoadRequest.assetBundle;
            List<Hash128> listOfCachedVersions = new List<Hash128>();
            Caching.GetCachedVersions(bundle.name, listOfCachedVersions);
            Debug.Log("######################" + listOfCachedVersions.Count +" "+ bundle.name);
            if (bundle is not null)
            {
                lm.assetBundle.Insert(0, bundle);
            }
            else
            {
                Debug.Log("Failed to load AssetBundle!");
                //yield break;
            }
        }
        Debug.Log("lm.assetBundle.Count == 0" +(lm.assetBundle.Count == 0).ToString());
        if (lm.assetBundle.Count == 0)
        {
            foreach (BundleData bundD in bundleData)
            {
                StartCoroutine(LoadData("new", bundD));
            }
        }
    }
    void GetBundleData(string request)
    {
        string apiRequestStr = request;
        List<string> jsonStrs = new List<string>();
        string pat = @"[\[\]]"; // delete '[' or ']'
        string patDif = @"\},\{"; // replace ',' between dicts

        apiRequestStr = (Regex.Replace(apiRequestStr, pat, String.Empty));
        apiRequestStr = (Regex.Replace(apiRequestStr, patDif, @"};{"));
        jsonStrs = apiRequestStr.Split(';').ToList();

        foreach (string st in jsonStrs)
        {
            bundleData.Add(JsonConvert.DeserializeObject<BundleData>(st));
            Debug.Log(bundleData[0].version + " " + bundleData[0].id + " " + bundleData[0].bundleName);
            Debug.Log(st);
        }
    }

    // можно упростить убрав versionType, по идее ее перекрывает загрузка из кэша
    IEnumerator LoadData(string versionType, BundleData bundleData)
    {
        Debug.Log("-=/{[############]}\\=- LOAD DATA -=/{[############]}\\=-");
        Debug.Log(versionType + bundleData.bundleName);
        bool flagErr = true;
        Debug.Log(assetBundle.Count + " " + bundleData.bundleName.Split(',').Length);

        //server
        //string bundleUrl = rootUrl + "assetbundle/" + bundleData.bundleName;
        //google
        string bundleUrl = rootUrl + "uc?export=download&id=" + bundleData.bundleName;

        Debug.Log("link " + bundleUrl);
        Hash128 hash = new Hash128();
        if (versionType == "new")
            hash.Append(bundleData.version);
        else if ((versionType == "old"))
            hash.Append(PlayerPrefs.GetInt(bundleData.bundleName + "_version"));
        hash.Append(bundleUrl);
        Debug.Log(hash);
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl, new CachedAssetBundle(cacheFolder+ bundleData.bundleName, hash), 0))
        //using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl, (uint)PlayerPrefs.GetInt(bundleData.bundleName + "_version"), 0))
        {
            //string uwrJson = JsonConvert.SerializeObject(uwr);
            //Debug.Log(uwrJson);
            yield return uwr.SendWebRequest();
            try
            {
                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    Debug.Log(uwr.isDone);
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);
                    bool success = Caching.ClearAllCachedVersions(bundle.name);

                    if (!success)
                    {
                        Debug.Log("Unable to clear the caches");
                    }

                    if (bundle is not null)
                    {
                        lm.assetBundle.Insert(0, bundle);
                    }

                    Debug.Log(bundle.ToString());
                    foreach (var d in bundle.GetAllAssetNames())
                    {
                        Debug.Log(d);
                    }
                    flagErr = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }

        foreach (var ab in assetBundle)
        {
            Debug.Log(assetBundle.Count + ab.ToString());
        }
        if (!flagErr && versionType == "new")
        {
            PlayerPrefs.SetInt(bundleData.bundleName + "_version", (int)bundleData.version);
        }

    }

    IEnumerator checkConnection(string uri, Action<bool> action)
    {
        UnityWebRequest www = new UnityWebRequest(uri);
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }
}
