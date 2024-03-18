using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelPieceDocBtn : MonoBehaviour
{
    Button btn;
    public Sprite sprite;
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowDoc);
    }

    public void ShowDoc()
    {
        Debug.Log("ShowDoc");
        ModelTemplateUIHandler.Instance.docsHandler.SetPage(sprite);
        ModelTemplateUIHandler.Instance.docsHandler.gameObject.SetActive(true);
    }
}
