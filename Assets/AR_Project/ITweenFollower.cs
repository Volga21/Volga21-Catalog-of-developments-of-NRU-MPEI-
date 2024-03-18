using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITweenFollower : MonoBehaviour
{
    public string pathName;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathName), "easytype", iTween.EaseType.easeOutSine, "time", time));

    }

    // Update is called once per frame
    void Update()
    {
        iTween.RotateUpdate(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathName), "easytype", iTween.EaseType.easeOutSine, "time", time));
    }
}
