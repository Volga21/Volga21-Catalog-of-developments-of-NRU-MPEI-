using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
/// <summary>
/// Класс родитель для всех классов моделей
/// </summary>
public class ObjectModel : MonoBehaviour
{
    //строка описания модели
    public string description;
    public string modelName;

    // функционал(информация) отдельно взятой модели разработки
    public bool hasShortDescr = false;
    public bool canSlice;
    public bool hasAnimation;
    public bool hasVideo;
    public bool hasDoc;
    public bool hasLink;

    //// материал модели
    //public Material mat;

    // список сотавных частей, на которые разбирается модель
    [SerializeField]
    protected List<MovablePart> parts;
    // состояние модели
    protected bool isGathered = true;

    // страницы(изображения) документации 
    [SerializeField]
    public Sprite[] pages;
    public VideoClip videoClip;
    public List<Material> materialsToSlice = new List<Material>();

    [Header("======= Links =======")]
    public List<string> URLNames = new List<string>();
    public List<string> URLLinks = new List<string>();


    Animator animator;
    [Header("======= Animation =======")]
    [Range(0, 5)]
    public int currentAnimateState = 0;
    public List<string> animationNames;
    //public AnimatorOverrideController overrideController;

    protected virtual void Awake()
    {
        if (hasAnimation)
        {
            animator = GetComponent<Animator>();
            if (animator is not null)
            {
                animator.SetBool(currentAnimateState.ToString(), true);
            }
        }

        //if (hasAnimation)
        //{
        //    animator = GetComponent<Animator>();
        //    if (animator != null)
        //    {
        //        animator.runtimeAnimatorController = overrideController;
        //    }
        //}
        //if (GetComponent<Renderer>())
        //{
        //    mat = GetComponent<Renderer>().material;
        //}

        //foreach (Transform item in this.transform)
        //{
        //    if (item.GetComponent<MovablePart>())
        //    {
        //        parts.Add(item.GetComponent<MovablePart>());
        //    }

        //    if (canSlice)
        //    {
        //        item.gameObject.AddComponent(typeof(SimpleCutDetail));
        //    }
        //}
    }

    public void SetMatSlicePlane(float val)
    {
        foreach (Material mat in materialsToSlice)
        {
            mat.SetFloat("_Dissolve", val);
        }
    }

    public void SetMatSliceToggle(int fX, int fY, int fZ, float disTh)
    {
        foreach (Material mat in materialsToSlice)
        {
            mat.SetInt("_FlagX", fX);
            mat.SetInt("_FlagY", fY);
            mat.SetInt("_FlagZ", fZ);
            mat.SetFloat("_DissolveThickness", disTh);
        }
    }


    public void Decomposition()
    {
        if (parts.Count != 0)
        {
            foreach (MovablePart item in parts)
            {
                item.Split(isGathered);
            }
        }
        isGathered = !isGathered;
    }

    public void Animate(int state)
    {
        if (animator is not null)
        {
            if (state != currentAnimateState)
            {
                animator.SetBool(currentAnimateState.ToString(), false);
                currentAnimateState = state;
            }
            animator.SetBool(currentAnimateState.ToString(), true);
        }
    }
}
