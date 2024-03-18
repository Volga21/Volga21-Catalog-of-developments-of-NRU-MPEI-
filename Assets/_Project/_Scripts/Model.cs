using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Model : MonoBehaviour
{
    [Header("Интерактивные объекты.")]
    [Space(5)]
    public List<Interactable> interactableObjects = new List<Interactable>();

    [Space(10)]
    [Header("Объекты для режима рабочих параметров.")]
    [Space(5)]
    public List<DT_Point> DTPointsOfModel = new List<DT_Point>();

    [Space(10)]
    [Header("Описание разработки.")]
    [Space(5)]
    [TextArea(2, 5)]
    public string descriptionPanelText;
    [Space]

    // Компонент отвечающий за Анимацию.
    public AnimatorManager AnimatorManager;

    // Визуальные эффекты.
    public GameObject VFXObj;

    // Настройка модели и интерактивных объектов.
    public void SetUPModel()
    {
        foreach (var DT_Point in DTPointsOfModel)
        {
            DT_InfoPanel newPanel = Instantiate(DT_Point.InfoPanelPrefab);
            newPanel.gameObject.transform.SetParent(AppManager.Instanse.MainUI.gameObject.transform);
            newPanel.transform.SetAsFirstSibling();
            newPanel.Target = DT_Point.gameObject.transform;
            DT_Point.InstantiatedInfoPanel = newPanel;
        }

        foreach (var interactableObj in interactableObjects)
        {
            interactableObj.SetUPInteractable();
        }

        DiactivateAllInfoPanels();

        AnimatorManager = GetComponent<AnimatorManager>();
    }

    // Включение всех панелей с параметрами.
    public void ActivateAllInfoPanels()
    {
        foreach (var DT_Point in DTPointsOfModel)
        {
            DT_Point.InstantiatedInfoPanel.gameObject.SetActive(true);
        }
    }
    // Выключение всех панелей с параметрами.
    public void DiactivateAllInfoPanels()
    {
        foreach (var DT_Point in DTPointsOfModel)
        {
            DT_Point.InstantiatedInfoPanel.gameObject.SetActive(false);
        }
    }

    // Методы нужные для интерактивных кнопок.
    public void EnterViewMode()
    {
        AppManager.Instanse.EnterViewWithDescriptionMode();
    }

    public void EnterDigitalTwinMode()
    {
        AppManager.Instanse.EnterWorkOptionsMode();
    }

    // Методы для включения/отключения возможности взаимодействия с моделью.
    public void EnableRayCast()
    {
        AppManager.Instanse.CanTrace = true;
    }

    public void DiableRayCast()
    {
        AppManager.Instanse.CanTrace = false;
    }



    // Включает визуальные эффекты.
    public void ShowVFX()
    {
        if (VFXObj)
        {
            VFXObj.SetActive(true);
        }
    }

    // Выключает визуальные эффекты.
    public void HideVFX()
    {
        if (VFXObj)
        {
            VFXObj.SetActive(false);
        }
    }


}
