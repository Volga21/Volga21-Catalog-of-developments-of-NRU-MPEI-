using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliceOnePlane : MonoBehaviour
{
    public Toggle YZ;
    public Toggle XZ;
    public Toggle XY;
    public GameObject plane;
    public Slider slider;

    List<float> startPosition = new List<float>();
    List<float> length = new List<float>();

    List<float> matIncVal = new List<float>();
    List<float> matStartValue = new List<float>();

    public Material[] maters;
    public int toggleInd = 0;
    private void Awake()
    {
        matIncVal = new List<float> { 5.0f, -2.45f, 5.0f };
        matStartValue = new List<float> { -2f, 3f, -2f };
    }
    private void Start()
    {
        RefreshPositon();
    }
    public void RefreshPositon()
    {
        //plane.transform.rotation = Quaternion.Euler(-90, 0, 90);
        //startPosition = new List<float> { -5, 0, 0 };
        slider = gameObject.GetComponent<Slider>();
        YZ.isOn = true;
        ToggleChanged(0);
        slider.value = 0.0f;
        foreach (Material mat in maters)
        {
            mat.SetFloat("_Dissolve", matStartValue[0] + slider.value * matIncVal[0]);
        }
    }
    void Update()
    {
        plane.transform.position = new Vector3(
            startPosition[0] + slider.value * length[0],
            startPosition[1] + slider.value * length[1],
            startPosition[2] + slider.value * length[2]);
        Spawner.Instance.objectModel.SetMatSlicePlane(matStartValue[toggleInd] + slider.value * matIncVal[toggleInd]);
        //foreach (Material mat in maters)
        //{
        //    mat.SetFloat("_Dissolve", matStartValue[toggleInd] + slider.value * matIncVal[toggleInd]);
        //}
        //Debug.Log(matStartValue[toggleInd] + slider.value * matIncVal[toggleInd]);
        //Debug.Log(toggleInd);
    }
    void SetEnviro(Quaternion angle, List<float> startPos, List<float> len, Toggle t1off, Toggle t2off, int fX, int fY, int fZ, float disTh, int ind)
    {
        plane.transform.rotation = angle;
        startPosition = startPos;
        length = len;
        t1off.isOn = false;
        t2off.isOn = false;

        Spawner.Instance.objectModel.SetMatSliceToggle(fX,fY,fZ,disTh);
        //foreach (Material mat in maters)
        //{
        //    mat.SetInt("_FlagX", fX);
        //    mat.SetInt("_FlagY", fY);
        //    mat.SetInt("_FlagZ", fZ);
        //    mat.SetFloat("_DissolveThickness", disTh);
        //}
        toggleInd = ind;
    }
    public void ToggleChanged(int num) //x
    {
        if (num == 0 && YZ.isOn)
        {
            SetEnviro(Quaternion.Euler(-90, 0, 90),
                new List<float> { -5f, 0, 0 },
                new List<float> { 10f, 0, 0 },
                XZ, XY, 1, 0, 0,
                0.05f, num);
        }
        else if (num == 1 && XZ.isOn) //y
        {
            SetEnviro(Quaternion.Euler(0, 0, 0),
                new List<float> { 0, 5f, 0 },
                new List<float> { 0, -5f, 0 },
                YZ, XY, 0, 1, 0,
                -0.05f, num);
        }
        else if (num == 2 && XY.isOn) //z
        {
            SetEnviro(Quaternion.Euler(-90, 0, 0),
                new List<float> { 0, 0, -5f },
                new List<float> { 0, 0, 10f },
                YZ, XZ,
                0, 0, 1,
                0.05f, num);
        }
        slider.value = 0.0f;
    }

}
