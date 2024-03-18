using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocsHandler : MonoBehaviour
{
    public GameObject prevButton;
    public GameObject nextButton;

    public Image img;
    public ModelTemplateUIHandler modelTemplateUIHandler;
    int currentPage = 0;

    public void RefreshDocUI()
    {
        prevButton.SetActive(false);
        if (Spawner.Instance.objectModel.pages.Length > 1)
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
        currentPage = 0;
        SetPage(currentPage);
    }
    //private void Start()
    //{
    //    RefreshDocUI();
    //}
    public void SetPage(int pageN)
    {
        img.sprite = Spawner.Instance.objectModel.pages[pageN];
    }
    public void SetPage(Sprite sprt)
    {
        img.sprite = sprt;
        prevButton.SetActive(false);
        nextButton.SetActive(false);
    }
    public void ShowButton()
    {
        if (currentPage == 0)
        {
            prevButton.SetActive(false);
            nextButton.SetActive(true);
        }
        else if (currentPage == Spawner.Instance.objectModel.pages.Length - 1)
        {
            prevButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
            nextButton.SetActive(true);
        }
    }
    public void PrevPage()
    {
        currentPage -= 1;
        if (currentPage < 0)
        {
            currentPage = 0;
            
        }
        ShowButton();
        SetPage(currentPage);
    }
    public void NextPage()
    {
        currentPage += 1;
        int lastPage = Spawner.Instance.objectModel.pages.Length - 1;
        if (currentPage > lastPage)
        {
            currentPage = lastPage;
        }
        ShowButton();
        SetPage(currentPage);
    }
}
