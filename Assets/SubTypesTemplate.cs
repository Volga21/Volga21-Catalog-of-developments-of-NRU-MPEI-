using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubTypesTemplate : MonoBehaviour
{
    public CollectionUIHandler uIHandler;
    Button btn;
    public int ind;
    public TMP_Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SelectType);
    }
    void SelectType()
    {
        uIHandler.FillTypeList(ind);
    }
}
