using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiAnimBtn : MonoBehaviour
{
    private Button button;
    public ModelTemplateUIHandler handler;
    public TMP_Text btnName;

    public void Setup(int s, string nm)
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(() => { Spawner.Instance.objectModel.Animate(s); });
        btnName.text = nm;
    }

}
