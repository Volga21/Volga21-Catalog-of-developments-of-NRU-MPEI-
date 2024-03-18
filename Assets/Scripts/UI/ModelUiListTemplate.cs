using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModelUiListTemplate : MonoBehaviour
{
    Button btn;

    public CollectionUIHandler CollectionUI;
    public TMP_Text ModelNameTxt;
    public int TypeId;
    public int ModelId;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectModel);
    }

    void SelectModel()
    {
        CollectionUI.SelectModel(TypeId, ModelId);
    }
}
