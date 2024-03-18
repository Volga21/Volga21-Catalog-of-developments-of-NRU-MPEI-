using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Networking;

public class IDTransformer : MonoBehaviour
{
    //string nmKey = "1HydrogenEnergy";

    //// Start is called before the first frame update
    //async Task Start()
    //{
    //    Debug.Log("start");
    //    //Returns any IResourceLocations that are mapped to the key "AssetKey"
    //    AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(nmKey);
    //    await handle.Task;
    //    Debug.Log("Task");
    //    foreach (IResourceLocation location in handle.Result)
    //    {
    //        test(location);
    //    }

    //    //Addressables.Release(handle);
    //}

    [RuntimeInitializeOnLoadMethod]
    static void SetInternalIdTransform()
    {
        Addressables.InternalIdTransformFunc = MyCustomTransform;
    }
    static int i=0;
    public bool hasFile=true;
    string address;
    public  static string MyCustomTransform(IResourceLocation location)
    {
        if (location.ResourceType == typeof(IAssetBundleResource)
            && location.InternalId.StartsWith("http"))
        {
            
            int index = location.InternalId.LastIndexOf('/');
            //string s = location.InternalId.Remove(index, 1);
            string s = location.InternalId;
            Debug.Log($"({i}) "+s);
            i++;
            return s + "?alt=media";
        }

        return location.InternalId;
    }
    public void check(){
        StartCoroutine(checkFile(address));
    }
    IEnumerator checkFile(string s){
        UnityWebRequest request=UnityWebRequest.Get(address);
        yield return request.SendWebRequest();
        if(request.isNetworkError==false){
            hasFile=false;
        }
    }
    //void test(IResourceLocation location)
    //{
    //    //Debug.Log(location.InternalId);
    //}
}
