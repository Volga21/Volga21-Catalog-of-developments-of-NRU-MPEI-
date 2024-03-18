using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LinkHandler : MonoBehaviour
{
    public TMP_Text urlName;
    public TMP_Text urlLink;
    public Button linkBtn;

    public void Init(string name, string link)
    {
        urlName.text = name;
        urlLink.text = link;
        linkBtn.onClick.AddListener(() => Application.OpenURL(link));
    }
}
