using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public GameObject loadingScreen;
    public List<string> facts = new List<string>(){"МЭИ - лучший Энерегтический вуз страны!","Не забывайте отдыхать!","Не забывайте про неделю контрольных мероприятий!"};
    //public GifAutoPlay autoPlay;

    public void ChangeScene(string s)
    {
        StartCoroutine(LoadAsync(s));
    }
    IEnumerator LoadAsync(string s)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName: s);
        //autoPlay.Play();

        loadingScreen.SetActive(true);
        StartCoroutine(setFacts(operation));
        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //loadProcent.text = (operation.progress * 100).ToString();
            ////Debug.Log(progress);
            //slider.value = progress;

            yield return null;
        }

    }
    IEnumerator setFacts(AsyncOperation operation){
        var panel = loadingScreen.transform.GetChild(1).gameObject;
        var fact = panel.transform.GetChild(0).gameObject;
        var textTMP = fact.GetComponent<TMP_Text>();
        while(!operation.isDone){
            textTMP.SetText(facts[0]);
            yield return new WaitForSeconds(2);
            textTMP.SetText(facts[1]);
            yield return new WaitForSeconds(2);
            textTMP.SetText(facts[2]);
            yield return new WaitForSeconds(2);
        }
        yield return null;
    }

}
