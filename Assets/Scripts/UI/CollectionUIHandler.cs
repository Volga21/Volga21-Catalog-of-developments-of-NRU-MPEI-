using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using System;
using Newtonsoft.Json;
using TMPro;
using System.IO;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;

    public class CollectionUIHandler : MonoBehaviour
    {

    public GameObject MainUIElement;
    public MainTypesListTemplate mTemplate;
    List<JsPrefabs> labels=new List<JsPrefabs>();

    [SerializeField] public GameObject Spaw;

    public GameObject loadingScreen;
    public GameObject loadScreen;

    [SerializeField]
    private bool SetupReady = false;

    [Header("MainModelTypes")]
    public GameObject mainModelTypesUI;
    public GameObject mainModelTypesListUI;
    public List<MainTypesListTemplate> mainTypesList = new List<MainTypesListTemplate>();
    public GameObject downloadBtn;
     public List<string> facts = new List<string>(){"МЭИ - лучший Энерегтический вуз страны!","Не забывайте отдыхать!","Не забывайте про неделю контрольных мероприятий!"};

    [Header("SubModelTypes")]
    public GameObject subMenu;
    public GameObject content;
    public GameObject btnCollection;
    public GameObject subTemplate;
    public GameObject ModelName;    
    //public Button subBackBtn;
    //public List<Sprite> modelTypesIcon= new List<Sprite>();

    [Header("TypesList")]
    public GameObject TypesListUI;
    public GameObject TypesListList;
    public GameObject TypesListListTemplate;
    public GameObject typeListMenu;
    public TMP_Text TypeTxtList;
    public Button typesBackBtn;

    public GameObject TogglePrefab;
    public GameObject ToggleCanvas;
    bool was=false;
    bool Updated=false;

    public GameObject error;

    private IEnumerator coroutine;


    void Start()
    {
       Debug.Log("Collection");
        Caching.ClearCache();
       StartCoroutine(setFacts());
       StartCoroutine(WaitMoledSetup());
       
    }
    IEnumerator WaitMoledSetup()
    {
        string errorString="";
        while (!SetupReady)
            {
                SetupReady = true;
                foreach (var type in mainTypesList)
                {
                    if (type.setupIsDone == false)
                    {
                        SetupReady = false;
                    }
                }
                if (SetupReady) { Debug.Log("setup ready "); loadScreen.gameObject.SetActive(false); }
               yield return null;
            }
        var panel2 = error.transform.GetChild(1).gameObject;
        var panel3= panel2.transform.GetChild(0).gameObject;
        var panel4= panel3.transform.GetChild(0).gameObject;
        var panel5= panel4.transform.GetChild(0).gameObject;
        var panel6= panel5.transform.GetChild(0).gameObject;
        var textTMP = panel6.GetComponent<TMP_Text>();
        bool flag=true;
        bool flag2=true;
        string er="";
        foreach(var type in  mainTypesList){
            if(flag && type.internetConnection==false){
                flag=false;
            }
            if(type.catalog.Length!=0){
                er+=type.catalog+", ";
            }
            if(type.error.Length!=0)
                er+=type.error+", ";
        }
        if(er.Length!=0)
            er=er.Substring(0,er.Length-2)+".";
        if(!flag && er.Length!=0){
            error.SetActive(true);
            textTMP.SetText($"Нет подключения к интернету, не удалось загрузить каталог для обновления. Не удалось загрузить: {er}");
        }
        else
        {
            if(!flag && er.Length==0){
                error.SetActive(true);
                textTMP.SetText($"Нет подключения к интернету, не удалось загрузить каталог для обновления.");
            }
            else{
                if(flag && er.Length!=0){
                    error.SetActive(true);
                    textTMP.SetText($"Не удалось загрузить: {er}");
                }
            }
        }
    }
    IEnumerator setFacts(){
        var panel = loadScreen.transform.GetChild(1).gameObject;
        var fact = panel.transform.GetChild(0).gameObject;
        var textTMP = fact.GetComponent<TMP_Text>();
        while(!SetupReady){
            textTMP.SetText(facts[1]);
            yield return new WaitForSeconds(3);
            textTMP.SetText(facts[2]);
            yield return new WaitForSeconds(3);
            textTMP.SetText(facts[0]);
            yield return new WaitForSeconds(3);
        }
        yield return null;
    }

    

    public void FillTypeList(int ind)
    {
        TypeTxtList.text = mTemplate.typeName;
        foreach (Transform child in TypesListList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < mTemplate.objectModelPrefabs[ind].sampleList.Count; i++)
        {
            GameObject template = Instantiate(TypesListListTemplate, TypesListList.transform);
            ModelUiListTemplate tmpl = template.GetComponent<ModelUiListTemplate>();
            tmpl.CollectionUI = this;
            tmpl.ModelNameTxt.text = mTemplate.objectModelPrefabs[ind].sampleList[i].modelName;
            tmpl.TypeId = ind;
            tmpl.ModelId = i;
        }
        typeListMenu.SetActive(true);
    }
    public void SelectModel(int t, int m)
    {
        gameObject.SetActive(false);
        Debug.Log($"t={t},m={m}");
        btnCollection.SetActive(true);
        ModelName.SetActive(true);
        Debug.Log($"size of objectModelPrefabs {mTemplate.objectModelPrefabs.Count}");

        Spawner.Instance.SpawnObject(mTemplate.objectModelPrefabs[t].sampleList[m]);
    }
    public void SelectType(int t)
    {
        mainModelTypesUI.SetActive(false);
        TypesListUI.SetActive(true);
        FillTypeList(t);
    }
    void FillSubList()
    {
        TypeTxtList.text = mTemplate.typeName;
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < mTemplate.subTypes.Count; i++)
        {
            GameObject template = Instantiate(subTemplate, content.transform);
            SubTypesTemplate tmpl = template.GetComponent<SubTypesTemplate>();
            tmpl.uIHandler = this;
            tmpl.ind = i;
            tmpl.buttonText.text = mTemplate.subTypes[i];
        }
    }

    public void FillContentToInstallOrUpdate()
    {
        //Очистка меню загрузки
        for (int i=ToggleCanvas.transform.childCount-1;i>-1;i--){
            Destroy(ToggleCanvas.transform.GetChild(i).gameObject);
        }   
        for (int i=0;i<mTemplate.updatePrefabs.Count;i++){
            GameObject newToggleButton = Instantiate(TogglePrefab,ToggleCanvas.transform);
            var textobj = newToggleButton.transform.GetChild(0).gameObject;
            var tmpText = textobj.GetComponent<TMP_Text>();
            if(mTemplate.subTypes.Count==2){
                tmpText.SetText(mTemplate.updatePrefabs[i].Name+" "+mTemplate.subTypes[mTemplate.updatePrefabs[i].SubVersion]);
            }
            else{
                tmpText.SetText(mTemplate.updatePrefabs[i].Name);
            }
        
            Toggle t= newToggleButton.GetComponent<Toggle>();
            for(int j=0;j<mTemplate.hasPrefabs.Count;j++){
                if(mTemplate.hasPrefabs[j].Version==mTemplate.updatePrefabs[i].Version && mTemplate.hasPrefabs[j].Name==mTemplate.updatePrefabs[i].Name){
                    t.interactable=false;
                    if(mTemplate.subTypes.Count==2){
                        tmpText.SetText(mTemplate.hasPrefabs[j].Name+" "+mTemplate.subTypes[mTemplate.hasPrefabs[j].SubVersion]+" <b>Скачано</b>");
                    }
                    else{
                        tmpText.SetText(mTemplate.hasPrefabs[j].Name+" <b>Скачано</b>");
                    }
                    break;
                }
                else if (mTemplate.hasPrefabs[j].Version<mTemplate.updatePrefabs[i].Version && mTemplate.hasPrefabs[j].Version==mTemplate.updatePrefabs[i].Version){
                    if(mTemplate.subTypes.Count==2){
                        tmpText.SetText(mTemplate.hasPrefabs[j].Name+" "+mTemplate.subTypes[mTemplate.hasPrefabs[j].SubVersion]+" <b>Обновление</b>");
                    }
                    else{
                        tmpText.SetText(mTemplate.hasPrefabs[j].Name+" <b>Обновление</b>");
                    }
                    break;
                }
            }
        }
        //typeListMenu.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1040f, MainUIElement.GetComponent<RectTransform>().rect.height - 200f);
    }
    public void FillContentToDelete(){
        for (int i=ToggleCanvas.transform.childCount-1;i>-1;i--){
            Destroy(ToggleCanvas.transform.GetChild(i).gameObject);
        }
            int x=750;
            int y=-50;
            for (int i=0;i<mTemplate.hasPrefabs.Count;i++){
                GameObject newToggleButton = Instantiate(TogglePrefab,ToggleCanvas.transform);
                var textobj = newToggleButton.transform.GetChild(0).gameObject;
                var tmpText = textobj.GetComponent<TMP_Text>();
                tmpText.SetText(mTemplate.hasPrefabs[i].Name);
                Toggle t= newToggleButton.GetComponent<Toggle>();
                newToggleButton.transform.SetParent(ToggleCanvas.transform, false);
            }
    }
    public void installPrefabs(){
        labels.Clear();
        double size=0;
        for(int i=0;i<ToggleCanvas.transform.childCount;i++){
            GameObject go = ToggleCanvas.transform.GetChild(i).gameObject;
            Toggle t=go.GetComponent<Toggle>();
            if(t.isOn==true){
                Debug.Log(mTemplate.updatePrefabs[i].Name);
                size+=mTemplate.updatePrefabs[i].Size;
                labels.Add(mTemplate.updatePrefabs[i]);
            }
        }
        Debug.Log($"размер файлов {size}");
        loadingScreen.SetActive(true);
        var panel = loadingScreen.transform.GetChild(1).gameObject;
        var temp = panel.transform.GetChild(0).gameObject;
        var text= temp.GetComponent<TMP_Text>();
        if(size!=0){
            text.SetText($"Будет скачано {size} МегаБайт\nСкачать?"); 
            Debug.Log($"{labels.Count} -файлов будет скачано");  
        }
        else{
            loadingScreen.SetActive(false);
            var panel2 = error.transform.GetChild(1).gameObject;
            var panel3= panel2.transform.GetChild(0).gameObject;
            var panel4= panel3.transform.GetChild(0).gameObject;
            var panel5= panel4.transform.GetChild(0).gameObject;
            var panel6= panel5.transform.GetChild(0).gameObject;
            var button = panel.transform.GetChild(2).gameObject;
            button.GetComponent<Button>().onClick.Invoke();
            var textError = panel6.GetComponent<TMP_Text>();
            textError.SetText($"Вы ничего не выбрали");
            error.SetActive(true);
        }
    }
    public void yes(){
        var panel1 = loadingScreen.transform.GetChild(1).gameObject;
        panel1.SetActive(false);

        var panel = loadingScreen.transform.GetChild(2).gameObject;

        var go = panel.transform.GetChild(0).gameObject;
        Image slider=go.GetComponent<Image>();

        go = panel.transform.GetChild(1).gameObject;    
        var textTMP=go.GetComponent<TMP_Text>();

        go = panel.transform.GetChild(2).gameObject;    
        var textTMP1=go.GetComponent<TMP_Text>();

        textTMP.SetText("Загрузка");
        
        coroutine = startInstall(textTMP,textTMP1,slider,panel,labels);
        StartCoroutine(coroutine);
    }
    void deleteAndInsertGO(string name,ObjectModel objectM,JsPrefabs jsObject,GameObject objectG){
        bool flag=true;
        for (int i=0;i<labels.Count;i++){
            flag=true;
            for(int j=0;j<mTemplate.hasPrefabs.Count;j++){
                if(name==mTemplate.hasPrefabs[j].Name){
                    flag=false;
    
                    GameObject gm=mTemplate.objectModelKeys[jsObject.SubVersion].sampleList[j];

                    mTemplate.objectModelPrefabs[jsObject.SubVersion].sampleList.Remove(mTemplate.objectModelPrefabs[jsObject.SubVersion].sampleList[j]);
                    mTemplate.objectModelKeys[jsObject.SubVersion].sampleList.Remove(mTemplate.objectModelKeys[jsObject.SubVersion].sampleList[j]);

                    mTemplate.objectModelPrefabs[jsObject.SubVersion].sampleList.Insert(j,objectM);
                    mTemplate.objectModelKeys[jsObject.SubVersion].sampleList.Insert(j,objectG);

                    Addressables.ReleaseInstance(gm);
                   // DestroyImmediate(gm,true);

                    mTemplate.hasPrefabs[j]=jsObject;

                    break;
                }
            }
        }
        if(flag){
            mTemplate.hasPrefabs.Add(jsObject);
            mTemplate.objectModelPrefabs[jsObject.SubVersion].sampleList.Add(objectM);
            mTemplate.objectModelTemp[jsObject.SubVersion].sampleList.Add(objectG);
        }
    }
    public void stopInstall(string name,GameObject panel,TMP_Text text,TMP_Text text2,Image slider){
        StopCoroutine(coroutine);
        text.SetText("");
        text2.SetText("");
        slider.fillAmount=0f;
    //Addressables.Release(name);
        Addressables.ClearDependencyCacheAsync(name);
        loadingScreen.SetActive(false);
        var panel1 = loadingScreen.transform.GetChild(1).gameObject;
        panel1.SetActive(true);
        panel.SetActive(false);
        string json = JsonConvert.SerializeObject(mTemplate.hasPrefabs);
        Debug.Log(json);
        File.WriteAllText(mTemplate.path, json);
        Debug.Log("Отемена загрузки");
    }
    IEnumerator startInstall(TMP_Text text,TMP_Text text2,Image slider,GameObject panel,List<JsPrefabs> list){
        clearCache(list);
        bool internetConnection=true;
        var button=panel.transform.GetChild(3).gameObject;
        UnityWebRequest request=UnityWebRequest.Get("http://80.87.201.230/download/");
        yield return request.SendWebRequest();
        if(request.isNetworkError==true){
            internetConnection=false;
        }
        else{
            panel.SetActive(true);
            string errorMessage="";
            for(int i=0;i<list.Count;i++){
                string name=list[i].Name;
                    AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
                    if (handle.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError("Такого ключа нет !");
                    }
                    else{
                        Debug.Log("was here");
                        float percent=0;
                        slider.fillAmount=0f;
                        button.GetComponent<Button>().onClick.AddListener(()=>stopInstall(name,panel,text,text2,slider));
                        while(!handle.IsDone){
                                text2.SetText($"Загрузка {name}");
                                percent=handle.GetDownloadStatus().Percent;
                                slider.fillAmount=percent;
                                Debug.Log($"Загрузка {name} = {percent}" );
                                text.SetText($"{Math.Round(percent*100,2)}%");
                                yield return null;
                            }
                        if(handle.Status== AsyncOperationStatus.Succeeded){

                            mTemplate.objectModelKeys[list[i].SubVersion].sampleList.Add(handle.Result);

                            var handle2 = Addressables.InstantiateAsync(name, Spawner.Instance.transform);

                            if(handle2.Status==AsyncOperationStatus.Succeeded){
                                GameObject _cache = handle2.Result;
                                _cache.TryGetComponent(out ObjectModel component);
                                mTemplate.hasPrefabs.Add(list[i]);
                                mTemplate.objectModelPrefabs[list[i].SubVersion].sampleList.Add(component);
                                mTemplate.objectModelTemp[list[i].SubVersion].sampleList.Add(_cache);
                            }
                        }
                        else{
                            Debug.Log("Скачка не удалась");
                            if(errorMessage.Length!=0){
                                errorMessage+=", "+list[i].Name;
                            }
                            else{
                                errorMessage+=list[i].Name;
                            }
                            list.Remove(list[i]);
                            i--;
                        }
                    }
                }
            if(errorMessage.Length!=0){
                var panel2 = error.transform.GetChild(1).gameObject;
                var panel3= panel2.transform.GetChild(0).gameObject;
                var panel4= panel3.transform.GetChild(0).gameObject;
                var panel5= panel4.transform.GetChild(0).gameObject;
                var panel6= panel5.transform.GetChild(0).gameObject;
                var textTMP = panel6.GetComponent<TMP_Text>();
                error.SetActive(true);
                textTMP.SetText($"Загрузка {errorMessage} не удалась");
            }
        }
        loadingScreen.SetActive(false);
        var panel1 = loadingScreen.transform.GetChild(1).gameObject;
        panel1.SetActive(true); 
        panel.SetActive(false);
        if(!internetConnection){
            var panel2 = error.transform.GetChild(1).gameObject;
            var panel3= panel2.transform.GetChild(0).gameObject;
            var panel4= panel3.transform.GetChild(0).gameObject;
            var panel5= panel4.transform.GetChild(0).gameObject;
            var panel6= panel5.transform.GetChild(0).gameObject;
            var textTMP = panel6.GetComponent<TMP_Text>();
            error.SetActive(true);
            textTMP.SetText($"Нет интернета подключения");
        }
        string json = JsonConvert.SerializeObject(mTemplate.hasPrefabs);
        Debug.Log(json);
        File.WriteAllText(mTemplate.path, json);
        Debug.Log("Загрузка завершена");
        }
    public void clearCache(List<JsPrefabs> names){
        int ind=0;
        string json1 = JsonConvert.SerializeObject(names);
        bool flag;
        for(int i=0;i<names.Count;i++){
            Debug.Log(names[i].SubVersion);
            flag=false;
            for(int j=0;j<mTemplate.objectModelPrefabs[names[i].SubVersion].sampleList.Count;j++){
                if(names[i].Name==mTemplate.objectModelPrefabs[names[i].SubVersion].sampleList[j].modelName){
                    ind=j;
                    Debug.Log($"remove {names[i].Name}");
                    flag=true;
                    break;
                }
            }
            if(flag){
                Debug.Log($"{names[i].Name}=={mTemplate.objectModelPrefabs[names[i].SubVersion].sampleList[ind].modelName}");
                Addressables.ReleaseInstance(mTemplate.objectModelKeys[names[i].SubVersion].sampleList[ind]);
                Addressables.ReleaseInstance(mTemplate.objectModelTemp[names[i].SubVersion].sampleList[ind]);
                mTemplate.objectModelKeys[names[i].SubVersion].sampleList.RemoveAt(ind);
                mTemplate.objectModelTemp[names[i].SubVersion].sampleList.RemoveAt(ind);
                mTemplate.objectModelPrefabs[names[i].SubVersion].sampleList.RemoveAt(ind);
                StartCoroutine(ClearCacheDependency(names[i].Name));
               // Addressables.ClearDependencyCacheAsync(names[i].Name);
                mTemplate.hasPrefabs.Remove(names[i]);
            }
        }
        string json = JsonConvert.SerializeObject(mTemplate.hasPrefabs);
        Debug.Log($"hasPrefabs with out names {json}");
        File.WriteAllText(mTemplate.path, json);
    }
     IEnumerator ClearCacheDependency(string cacheKey)
    {
        // Очищает зависимость кэша для указанного ключа.
        var handle = Addressables.ClearDependencyCacheAsync(cacheKey,true);
        while(!handle.IsDone){
            yield return null;
        }
        yield return handle;
    }
    public void startDelete(){
        labels.Clear();
        for(int i=0;i<ToggleCanvas.transform.childCount;i++){
            GameObject go = ToggleCanvas.transform.GetChild(i).gameObject;
            Toggle t=go.GetComponent<Toggle>();
            var templ = go.transform.GetChild(0).gameObject;
            var textTMP=templ.GetComponent<TMP_Text>().text;
            Debug.Log(textTMP);
            if(t.isOn==true){
                labels.Add(mTemplate.hasPrefabs.Find(x=>x.Name==textTMP));
            }
        }
        loadingScreen.SetActive(true);
        var panel = loadingScreen.transform.GetChild(1).gameObject;
        var temp = panel.transform.GetChild(0).gameObject;
        var text= temp.GetComponent<TMP_Text>();
        if(labels.Count!=0){
            text.SetText($"Удалить {labels.Count} модели"); 
        }
        else{
            loadingScreen.SetActive(false);
            var panel2 = error.transform.GetChild(1).gameObject;
            var panel3= panel2.transform.GetChild(0).gameObject;
            var panel4= panel3.transform.GetChild(0).gameObject;
            var panel5= panel4.transform.GetChild(0).gameObject;
            var panel6= panel5.transform.GetChild(0).gameObject;
            var button = panel.transform.GetChild(2).gameObject;
            button.GetComponent<Button>().onClick.Invoke();
            var textError = panel6.GetComponent<TMP_Text>();
            error.SetActive(true);
            textError.SetText($"Вы ничего не выбрали"); 
        }
    }
    public void yesForDelete(){
        var panel1 = loadingScreen.transform.GetChild(0).gameObject;
        panel1.SetActive(false);

        clearCache(labels);
        loadingScreen.SetActive(false);
        panel1.SetActive(true);
    }
    public void installAll(){
        labels.Clear();
        double size=0;
        for(int i=0;i<ToggleCanvas.transform.childCount;i++){
            GameObject go = ToggleCanvas.transform.GetChild(i).gameObject;
            Toggle t=go.GetComponent<Toggle>();
            if(t.interactable==true){
                size+=mTemplate.updatePrefabs[i].Size;
                labels.Add(mTemplate.updatePrefabs[i]);
            }
        }
        Debug.Log($"размер файлов {size}");
        loadingScreen.SetActive(true);
        var panel = loadingScreen.transform.GetChild(1).gameObject;
        var temp = panel.transform.GetChild(0).gameObject;
        var text= temp.GetComponent<TMP_Text>();
        if(size!=0){
            text.SetText($"Будет скачано {size} килобайт\nСкачать?"); 
            Debug.Log($"{labels.Count} -файлов будет скачано");  
        }
        else{
            loadingScreen.SetActive(false);
            var panel2 = error.transform.GetChild(1).gameObject;
            var panel3= panel2.transform.GetChild(0).gameObject;
            var panel4= panel3.transform.GetChild(0).gameObject;
            var panel5= panel4.transform.GetChild(0).gameObject;
            var panel6= panel5.transform.GetChild(0).gameObject;
            var button = panel.transform.GetChild(2).gameObject;
            button.GetComponent<Button>().onClick.Invoke();
            var textError = panel6.GetComponent<TMP_Text>();
            error.SetActive(true);
            textError.SetText($"Вы уже все скачали");      
        }
     }
    public void DeleteAll(){
        labels.Clear();
        for(int i=0;i<ToggleCanvas.transform.childCount;i++){
            GameObject go = ToggleCanvas.transform.GetChild(i).gameObject;
            Toggle t=go.GetComponent<Toggle>();
            var templ = go.transform.GetChild(0).gameObject;
            var textTMP=templ.GetComponent<TMP_Text>().text;
            labels.Add(mTemplate.hasPrefabs.Find(x=>x.Name==textTMP));
        }
        loadingScreen.SetActive(true);
        var panel = loadingScreen.transform.GetChild(1).gameObject;
        var temp = panel.transform.GetChild(0).gameObject;
        var text= temp.GetComponent<TMP_Text>();
        if(labels.Count!=0){
            text.SetText($"Удалить {labels.Count} моедели"); 
        }
        else{
            loadingScreen.SetActive(false);
            var panel2 = error.transform.GetChild(1).gameObject;
            var panel3= panel2.transform.GetChild(0).gameObject;
            var panel4= panel3.transform.GetChild(0).gameObject;
            var panel5= panel4.transform.GetChild(0).gameObject;
            var panel6= panel5.transform.GetChild(0).gameObject;
            var button = panel.transform.GetChild(2).gameObject;
            button.GetComponent<Button>().onClick.Invoke();
            var textError = panel6.GetComponent<TMP_Text>();
            error.SetActive(true);
            textError.SetText($"Вы уже все удалили");
        }
    }
    public void FillTypeContent(MainTypesListTemplate mTLTemplate)//mTLTemplate- это я передаю из фалйа MainTypesListTemplate.cs
    {
        mainModelTypesUI.SetActive(false);
        TypesListUI.SetActive(true);
        mTemplate = mTLTemplate;
        if (mTemplate.objectModelPrefabs.Count == 1)
        {
            subMenu.SetActive(false);
            Debug.Log(mTemplate.name);
            FillTypeList(0);
            typeListMenu.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1040f, MainUIElement.GetComponent<RectTransform>().rect.height
                - 200f);
        }
        else
        {
            FillSubList();
            TypesListUI.SetActive(true);
            typeListMenu.SetActive(false);
            typeListMenu.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1040f, MainUIElement.GetComponent<RectTransform>().rect.height
                - 200f - subMenu.GetComponent<RectTransform>().rect.height);
            subMenu.SetActive(true);
        }
    }
}
//идеи сделать labels локальной переменной
