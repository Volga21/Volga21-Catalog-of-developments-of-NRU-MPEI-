using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
public class UiHandler : MonoBehaviour
{
    public CollectionUIHandler CollectionUI;
    public GameObject MediaContent;
    public GameObject ToMMenu;
    public GameObject PanelDesc;
    public GameObject PanelDocs;
    public GameObject loadingScreen;

    public List<string> facts = new List<string>(){"МЭИ - лучший Энерегтический вуз страны!","Не забывайте отдыхать!","Не забывайте про неделю контрольных мероприятий!"};

    int UILayer;

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == 5) // LayerMask.NameToLayer("UI") = 5
                return true;
        }
        return false;
    }

    public void changeScene(){
        StartCoroutine(LoadAsync());
    }
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
    IEnumerator LoadAsync()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync("SceneMainMenu");
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

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CollectionUI.gameObject.activeSelf)
            {
                if (CollectionUI.mainModelTypesUI.activeSelf)
                {
                    //CollectionUI.ModelTypesUI.SetActive(false);
                    CollectionUI.gameObject.SetActive(false);
                }
                else
                {
                    CollectionUI.mainModelTypesUI.SetActive(true);
                    CollectionUI.TypesListUI.SetActive(false);
                }
                //CollectionUI.gameObject.SetActive(false);

                return;
            }
            else if (MediaContent.activeSelf)
            {
                MediaContent.SetActive(false);

                return;
            }
            else if (PanelDesc.activeSelf) { PanelDesc.SetActive(false); }
            else if (PanelDocs.activeSelf) { PanelDocs.SetActive(false); }
            //else if (shortDescrUI.active) { shortDescrUI.SetActive(false); }
            else
            {
                ToMMenu.SetActive(true);
            }
        }*/
    }
}
