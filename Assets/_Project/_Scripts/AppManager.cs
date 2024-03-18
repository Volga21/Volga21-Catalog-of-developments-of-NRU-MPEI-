using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Touch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleSideMenu;
using UnityEngine.XR.ARFoundation;

public class AppManager : MonoBehaviour
{
    #region  Singleton

    private static AppManager _instance;
    public static AppManager Instanse { get { return _instance; } }

    #endregion

    [Header("_______________________AppMode_______________________")]
    [Space(5)]
    [Tooltip("При билде поставить на этот флаг false.(Он отвечает за тестирование приложения. True-тестирование на компьютере False - рабочий режим.)")]
    [SerializeField] public bool Debug = false;
    [Space(10)]

    #region  Variables / Переменные

    [Header("_______________________Список моделей_______________________")]
    [Space(5)]
    [SerializeField] private List<Model> _availableModels = new List<Model>();
    [Space(10)]


    [Space(5)]
    [Header("_______________________Модели на сцене_______________________")]
    [Space(5)]
    [SerializeField] private List<Model> _instantiatedModels = new List<Model>();
    [SerializeField] private List<Interactable> _interactableObjectsOnScene = new List<Interactable>();
    [Space(10)]
    [SerializeField] public Model _currentShownModel;

    [Space]
    [Header("_______________________Важные компоненты_______________________")]
    [Space]

    [Space(5)]
    [Header("Компоненты настраиваемые вручную")]

    public PlaceTrakedImages TrackedImagesManager;
    public LeanTouch LeanTochManager;
    public RayCastModels RayCastManager;

    [Space(5)]
    [Header("Компоненты настраиваемые автоматически")]
    public ARSession CurrentARSession;
    public HintHandler HintHandlerComponent;
    private Camera _mainCam;
    [HideInInspector] public Canvas MainUI;

    [Space(5)]
    [Header("_______________________UI Элементы_______________________")]
    [Space(5)]

    [Header("Горизонтальное окно с описанием")]
    [SerializeField] private RectTransform _descriptionPanelHorizontal;
    [SerializeField] private TextMeshProUGUI _descriptionPanelHorizontalText;
    [Space(10)]
    [Header("Вертикальное окно с описанием")]
    [SerializeField] private RectTransform _descriptionPanelVertical;
    [SerializeField] private TextMeshProUGUI _descriptionPanelVerticalText;

    [Space(10)]
    [Header("Боковое меню")]
    [SerializeField] private SimpleSideMenu _sideMenuComponent;

    [Space(10)]
    [Header("Курсор при выборе особенностей модели")]
    [SerializeField] private RectTransform _cursorSpecialDescriptionTransform;
    [SerializeField] private RectTransform _specialDescriptionPanelTransform;

    [Tooltip("Объект на сцене куда выводится имя модели")]
    [SerializeField] private TextMeshProUGUI _cursorDescriptionText;
    [SerializeField] private TextMeshProUGUI _cursorHeaderText;

    [SerializeField] private Image _imageComponenet;

    [Space(10)]
    [Header("_______________________Debug,тест_______________________")]
    [Space(5)]


    [Tooltip("True - метка найдена. False - метка не найдена.")]
    public bool CanCreate = false;
    [Tooltip("True - взаимодействие с интерактивными объектами. False - Выключен RayCast")]
    public bool CanTrace = true;
    [Space]
    [Header("Настройка материалов.")]
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _interactionMaterial;
    [SerializeField] public AppState CurrentAppState;
    [SerializeField] public Interactable CurrentInteractable;

    // Компоненты для проверки ориентации экрана.
    private ScreenOrientation _currentOrientation;
    private bool isVertical = false;
    private bool isHorizontal = false;

    #endregion


    #region  PlayerLoop/ Основные методы
    // Singleton    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // Компонеты.
        _mainCam = Camera.main;
        MainUI = FindObjectOfType<Canvas>();
        CurrentARSession = FindObjectOfType<ARSession>();
        HintHandlerComponent = FindObjectOfType<HintHandler>();
    }

    private void Start()
    {
        // Создание доступных моделей из списка.
        foreach (Model objToSpawn in _availableModels)
        {
            Model newObj = Instantiate(objToSpawn);
            newObj.SetUPModel();
            _instantiatedModels.Add(newObj);
            SetupInstantiatedObject(newObj);
            newObj.gameObject.SetActive(false);
        }

        // Включение режима просмотра особеностей.
        CurrentAppState = AppState.ViewDescription;
        ChangeCurrentState();
        // Закрытие всплывающей панельки слева.
        _sideMenuComponent.SetState(State.Closed);
    }

    // Добавление созданого объекта в листы уже созданных моделей на сцене и добавление его интерактивных объектов.
    public void SetupInstantiatedObject(Model newModel)
    {
        _interactableObjectsOnScene.AddRange(newModel.interactableObjects);

        foreach (Interactable objectOnScene in _interactableObjectsOnScene)
        {
            objectOnScene.SetUpInteractableMaterials(_defaultMaterial, _interactionMaterial);
            objectOnScene.SetUpTransform();
        }
    }

    private void Update()
    {
        // Проверка ориентации экрана.
        // Если ориентация вертикальная - скрывает горизонтальное окно с описанием и проявляет вертикальное.
        // Если ориентация горизонтальная - скрывает вертикальное окно с описанием и проявляет горизонтальное.
        _currentOrientation = Screen.orientation;

        if (_currentOrientation == ScreenOrientation.Portrait)
        {
            if (!isVertical)
            {
                if (CurrentAppState == AppState.ViewDescription && _descriptionPanelHorizontal.gameObject.activeSelf || _descriptionPanelVertical.gameObject.activeSelf)
                {
                    _descriptionPanelHorizontal.gameObject.SetActive(true);
                }
                _descriptionPanelVertical.gameObject.SetActive(false);
                isVertical = true;
                isHorizontal = false;
            }
        }

        if (_currentOrientation == ScreenOrientation.LandscapeLeft || _currentOrientation == ScreenOrientation.LandscapeRight)
        {
            if (!isHorizontal)
            {
                if (CurrentAppState == AppState.ViewDescription && _descriptionPanelHorizontal.gameObject.activeSelf || _descriptionPanelVertical.gameObject.activeSelf)
                {
                    _descriptionPanelVertical.gameObject.SetActive(true);
                }
                _descriptionPanelHorizontal.gameObject.SetActive(false);
                isHorizontal = true;
                isVertical = false;
            }
        }
    }

    // Метод вызываемый при обнаружении метки.
    public void ImageisSet()
    {
        _currentShownModel = _instantiatedModels[4];
        HintHandlerComponent.BeginHintCycle();
        CanCreate = true;
    }

    // Метод переключается между созданными на сцене объектами.
    public void ShowNewModel(int number)
    {
        if (CanCreate)
        {
            // Закрытие бокового меню.
            _sideMenuComponent.SetState(State.Closed);

            // Если до этого была показана другая модель
            // Скрывает все панели параметрво и курсоры, а также Vfx.
            if (_currentShownModel != null)
            {

                foreach (Interactable currentInteractable in _interactableObjectsOnScene)
                {
                    currentInteractable.CloseInteractable();
                    currentInteractable.ResetInteractable();
                    currentInteractable.DisableDisplayCursor();

                }
                _currentShownModel.DiactivateAllInfoPanels();
                _currentShownModel.gameObject.SetActive(false);
                _currentShownModel.HideVFX();
            }

            // Скрытие панелей с описанием модели.
            _descriptionPanelHorizontal.gameObject.SetActive(false);
            _descriptionPanelVertical.gameObject.SetActive(false);

            // Проявление модели.
            _currentShownModel = _instantiatedModels[number];
            _currentShownModel.gameObject.SetActive(true);
            _currentShownModel.gameObject.GetComponent<AnimatorManager>().StartApperAnim();

            // Для каждой интерактивной особенности включает курсоры.
            foreach (Interactable currentInteractable in _currentShownModel.interactableObjects)
            {
                currentInteractable.EnableDisplayCursor();
            }

            // Изменяет состояние программы на просмтор особенностей.
            CurrentAppState = AppState.ViewDescription;
            ChangeCurrentState();
        }
    }

    // Метод взаимодействия с интерактивной особеностью модели.
    public void InteractWithObject(Interactable desiredInteractable)
    {
        // Скрытие VFX.
        _currentShownModel.HideVFX();

        // Если при наэатии на особенность был открыт режим Рабочих параметров,
        // Переводит программу в режим просмотра особенностей.
        if (CurrentAppState == AppState.WorkOptions)
        {
            EnterViewWithDescriptionMode();
            SetCursorActive(true);
            _specialDescriptionPanelTransform.gameObject.SetActive(true);
            _cursorSpecialDescriptionTransform.gameObject.SetActive(true);
        }

        // Скрытие панелей с описанием.
        _descriptionPanelHorizontal.gameObject.SetActive(false);
        _descriptionPanelVertical.gameObject.SetActive(false);

        // Закрывает все интерактивные объекты.
        _currentShownModel.AnimatorManager.CloseInteractable();

        foreach (Interactable Object in _currentShownModel.interactableObjects)
        {
            if (Object == desiredInteractable)
            {
                Object.OpenInteractable();
                CurrentInteractable = Object;
            }
        }
    }

    // Метод возвращающий интерактивные особенности на свои места.
    public void LooseInteractableObject()
    {
        foreach (Interactable objectOnScene in _interactableObjectsOnScene)
        {
            if (objectOnScene.isActiveAndEnabled)
            {
                objectOnScene.CloseInteractable();
            }
        }

        CurrentInteractable = null;
        _currentShownModel.ShowVFX();
    }

    #endregion


    #region  StateMethods / Методы изменения Состояния
    public void EnterWorkOptionsMode()
    {
        _currentShownModel.HideVFX();

        foreach (Interactable currentInteractable in _currentShownModel.interactableObjects)
        {
            StopAnimateCurrentInteractable();
            currentInteractable.CloseInteractable();
        }

        _descriptionPanelHorizontal.gameObject.SetActive(false);
        _descriptionPanelVertical.gameObject.SetActive(false);
        CurrentAppState = AppState.WorkOptions;
        ChangeCurrentState();
    }

    public void EnterViewWithDescriptionMode()
    {
        _currentShownModel.ShowVFX();

        CurrentInteractable = null;

        foreach (Interactable currentInteractable in _currentShownModel.interactableObjects)
        {
            currentInteractable.CloseInteractable();
            currentInteractable.EnableDisplayCursor();
        }

        CurrentAppState = AppState.ViewDescription;
        ChangeCurrentState();

        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            _descriptionPanelVertical.gameObject.SetActive(false);
            _descriptionPanelHorizontal.gameObject.SetActive(true);
        }
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            _descriptionPanelVertical.gameObject.SetActive(true);
            _descriptionPanelHorizontal.gameObject.SetActive(false);
        }

        _descriptionPanelHorizontalText.text = _currentShownModel.descriptionPanelText;
        _descriptionPanelVerticalText.text = _currentShownModel.descriptionPanelText;
    }

    // Finite State Machine
    public void ChangeCurrentState()
    {
        if (CanCreate)
        {
            switch (CurrentAppState)
            {
                case (AppState.WorkOptions):
                    {
                        LeanTochManager.enabled = true;
                        RayCastManager.enabled = true;
                        _currentShownModel.ActivateAllInfoPanels();
                        SetCursorActive(false);
                        break;
                    }

                case (AppState.ViewDescription):
                    {
                        LeanTochManager.enabled = true;
                        RayCastManager.enabled = true;
                        _currentShownModel.DiactivateAllInfoPanels();
                        SetCursorActive(false);
                        foreach (Interactable currentInteractable in _currentShownModel.interactableObjects)
                        {
                            currentInteractable.EnableDisplayCursor();
                        }
                        break;
                    }
            }
        }

    }
    #endregion


    #region SupportMethods / Вспомогательные методы

    public void UpdateTextPanel(Interactable Object)
    {
        _imageComponenet.sprite = Object.imageToShow;
        _cursorDescriptionText.text = Object.GetDescription();
        _cursorHeaderText.text = Object.HeaderOfObject;
    }

    public void SetCursorActive(bool isOn)
    {
        if (CurrentAppState == AppState.ViewDescription)
        {
            _cursorSpecialDescriptionTransform.gameObject.SetActive(isOn);
            _specialDescriptionPanelTransform.gameObject.SetActive(isOn);
        }
        else
        {
            _cursorSpecialDescriptionTransform.gameObject.SetActive(false);
            _specialDescriptionPanelTransform.gameObject.SetActive(false);
        }

    }

    public void DisableAllCursors()
    {
        foreach (Interactable currentInteractable in _currentShownModel.interactableObjects)
        {
            currentInteractable.DisableDisplayCursor();
        }
    }

    public void SetCursorTransform(Vector3 newPos)
    {
        if (CurrentAppState == AppState.ViewDescription)
        {
            _cursorSpecialDescriptionTransform.position = _mainCam.WorldToScreenPoint(newPos);
        }
    }

    public void SetupCurrentModel(Vector3 newPosition, Quaternion newRotation)
    {
        _currentShownModel.gameObject.GetComponent<AnimatorManager>().StartApperAnim();
        _currentShownModel.gameObject.transform.SetPositionAndRotation(newPosition, newRotation);
    }

    public void PlaceCurrentModel(Vector3 newPosition)
    {
        _currentShownModel.gameObject.SetActive(true);
        _currentShownModel.gameObject.transform.position = newPosition;
    }

    #endregion


    #region  AnimationMethods / Методы Анимации
    public void AnimateCurrentInteractable()
    {
        _currentShownModel.AnimatorManager.StartInteract();
    }

    public void StopAnimateCurrentInteractable()
    {
        _currentShownModel.AnimatorManager.StopInteract();
    }

    public void ChooseAnimateCurrentInteractable(int amount)
    {
        _currentShownModel.AnimatorManager.ChooseInteractable(amount);
    }

    #endregion


    #region  ApplicationMethods / Методы приложения
    public void ResetScene()
    {
        CurrentARSession.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        CurrentARSession.enabled = false;
        SceneManager.LoadScene(0);
    }

    #endregion
}

public enum AppState
{
    WorkOptions,
    ViewDescription
}
