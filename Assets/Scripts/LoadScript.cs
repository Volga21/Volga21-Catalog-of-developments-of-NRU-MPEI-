using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour
{
    public string verChoise = "";
    //bool flag = true;
    public static LoadScript instance { get; private set; }
    public List<AssetBundle> assetBundle = new List<AssetBundle>();

    public GameLoader gameLoader;

    //private uint bundleVersion;
    //private string jsonStr = "";
    //private BundleData bundleData;

    public void setBundles()
    {
        //assetBundle = new List<AssetBundle>(gameLoader.assetBundle);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    //public void setChoisse(string s)
    //{
    //    verChoise = s;
    //}
    //IEnumerator LoadData(string ver)
    //{
    //    Debug.Log(ver);
    //    bool flagErr = true;
    //    Debug.Log(assetBundle.Count + " " + bundleData.bundles.Length);
    //    foreach (string link in bundleData.bundles)
    //    {
    //        Debug.Log("link" + link);
    //        Hash128 hash = new Hash128();
    //        if (ver == "n")
    //            hash.Append(bundleData.version);
    //        else if ((ver == "o"))
    //            hash.Append(PlayerPrefs.GetInt("version"));
    //        hash.Append(link);
    //        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(link, hash, 0))
    //        {
    //            yield return uwr.SendWebRequest();
    //            try
    //            {
    //                if (uwr.result != UnityWebRequest.Result.Success)
    //                {
    //                    Debug.Log(uwr.error);
    //                }
    //                else
    //                {
    //                    // Get downloaded asset bundle
    //                    Debug.Log(uwr.isDone);
    //                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

    //                    if (bundle is not null)
    //                    {
    //                        assetBundle.Insert(0, bundle);
    //                    }

    //                    Debug.Log(bundle.ToString());
    //                    //Debug.Log(bundle.GetAllAssetNames());
    //                    foreach (var d in bundle.GetAllAssetNames())
    //                    {
    //                        Debug.Log(d);
    //                    }
    //                }
    //            }
    //            catch (System.Exception e)
    //            {
    //                Debug.Log(e);
    //            }
    //            flagErr = false;
    //        }
    //    }
    //    foreach (var ab in assetBundle)
    //    {
    //        Debug.Log(assetBundle.Count + ab.ToString());
    //    }
    //    if (!flagErr && ver == "n")
    //    {
    //        PlayerPrefs.SetInt("version", (int)bundleData.version);
    //    }

    //}
    //IEnumerator GetData()
    //{
    //    UnityWebRequest www = new UnityWebRequest("https://drive.google.com/uc?export=download&id=1kgjtVneiR0YAE_7o-yby00u-vPq05phY");
    //    www.downloadHandler = new DownloadHandlerBuffer();
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        jsonStr = www.downloadHandler.text;
    //        bundleData = JsonConvert.DeserializeObject<BundleData>(jsonStr);
    //        //bundleVersion = bundleData.version;
    //        Debug.Log(bundleData.version);
    //        foreach (string st in bundleData.bundles)
    //        {
    //            Debug.Log(st);
    //        }
    //    }
    //    if (bundleData.version > PlayerPrefs.GetInt("version"))
    //    {
    //        versionPanel.SetActive(true);
    //    }
    //    //StartCoroutine(LoadData());
    //}
}
