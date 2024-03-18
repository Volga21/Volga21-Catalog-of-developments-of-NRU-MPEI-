using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitApp : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(quit);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quit();
        }
    }
    public void quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
