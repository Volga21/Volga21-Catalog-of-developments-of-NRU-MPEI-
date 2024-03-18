using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UiBtnScriptAnims : UiSliceBtnScript
{
    public GameObject animBtns;
    public GameObject animBtnTemplate;
    public override void Active()
    {
        DisplayHide();
        if (setActive)
        {
            handler.UpdateUi();
        }
        else
        {
            foreach (Transform child in animBtns.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            for (int i = 0; i <= Spawner.Instance.objectModel.animationNames.Count - 1; i++)
            {
                UiAnimBtn btn = Instantiate(animBtnTemplate, animBtns.transform).GetComponent<UiAnimBtn>();
                btn.handler = handler;
                btn.Setup(i, Spawner.Instance.objectModel.animationNames[i]);
            }

        }
    }

}
