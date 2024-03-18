using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        string layout = System.IO.File.ReadAllText("Assets/Has"+s+".json");
        hasPrefabs=JsonConvert.DeserializeObject<List<JsPrefabs>>(layout);
        StartCoroutine(LoadJson());
    }
     IEnumerator LoadJson(){
        AsyncOperationHandle<TextAsset> handle=Addressables.LoadAssetAsync<TextAsset>(s);
        handle.WaitForCompletion();
        yield return handle;
        if(handle.Status==AsyncOperationStatus.Succeeded){
            string json=handle.Result.text;
            Debug.Log(handle.Result.text);
            updatePrefabs= JsonConvert.DeserializeObject<List<JsPrefabs>>(json);
        }
    }*/

    // Update is called once per frame
    
}
