using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DescriptionBtn : MonoBehaviour
{
    Button btn;
    public GameObject pnl;
    public TMP_Text descTxt;
    public TMP_Text modelNameTxt;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowDesc);
    }
    
    void ShowDesc()
    {
        descTxt.text = Spawner.Instance.objectModel.description;
        modelNameTxt.text = Spawner.Instance.objectModel.modelName;
        pnl.SetActive(true);

    }
}
