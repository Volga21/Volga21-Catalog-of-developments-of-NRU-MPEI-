using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SceneManagment : MonoBehaviour
{
    public ARSession CurrentARSession;

    private void Awake()
    {
        CurrentARSession = FindObjectOfType<ARSession>();
    }
    public void ResetScene()
    {
        CurrentARSession.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
