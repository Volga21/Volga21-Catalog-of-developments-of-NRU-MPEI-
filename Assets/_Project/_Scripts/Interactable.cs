using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [Header("_______________________Настройки объекта_______________________")]
    [Space(5)]

    [SerializeField] public string HeaderOfObject;
    // Описание интерактивного объекта.
    [TextArea(2, 5)]
    public string ObjectDescription = "Введите описание интерактивного объекта";

    // Выводимая картинка.
    public Sprite imageToShow;

    [Header("UI элемент")]
    public GameObject CursorPrefab;

    [Space(10)]
    [Header("_______________________Настройки выбора_______________________")]
    [Space(5)]

    // Если у объекта только один интерактивный объект.
    public bool isAnimated = true;

    // Если у объекта несколько интерактивных объектов.
    public bool isMultipleParts = true;

    [Tooltip("Номер указывает к какой анимации перейти. 1 - первая. 2- вторая и тд.")]
    public int AnimatorNumberOfObject = 0;

    [Space(10)]
    // Cкорость вращение выбранного объекта.
    [Header("Скорость и угол вращение объекта")]
    [SerializeField]
    private Vector3 _rotation = new Vector3(0, 1, 0);

    // Mesh объекты для которых нужно менять материал. 
    [Header("Mesh у которых поменяется материал на голографический.")]
    public MeshRenderer[] ObjectsToChangeMaterial;

    [Space(10)]
    [Header("_______________________Debug/Тест_______________________")]
    [Space(5)]
    // Cтандартный материал.
    [SerializeField] private Material _defaultMaterial;
    // Материал при выборе объекта.
    [SerializeField] private Material _choosedMaterial;

    [SerializeField] private bool _isCursorSetUp = false;
    [SerializeField] private bool isChoosed = false;
    [SerializeField] private bool isRotated = false;

    private Quaternion _startRotation;
    private Quaternion _currentRotation;

    // Вращаемый объект
    [SerializeField] private Transform _rootTransform;

    // Коллайдер 
    private BoxCollider _boxCollider;

    // Созданный элемент UI
    private RectTransform _instantiatedCursor;

    // Настройка при создании модели.
    public void SetUPInteractable()
    {
        InstantiateUIHelper();
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Создание и преднастройка курсора. 
    private void InstantiateUIHelper()
    {
        var cursor = Instantiate(CursorPrefab, AppManager.Instanse.MainUI.transform);
        cursor.transform.SetAsFirstSibling();
        _instantiatedCursor = cursor.GetComponent<RectTransform>();
        _instantiatedCursor.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        _instantiatedCursor.gameObject.SetActive(false);
        _isCursorSetUp = true;
    }


    private void Update()
    {
        // Курсор следит за интерактивным объектом.
        _instantiatedCursor.transform.position = Camera.main.WorldToScreenPoint(_rootTransform.position);

        // Вращение объекта при выборе.
        if (isChoosed)
        {
            _rootTransform.Rotate(_rotation.x * Time.deltaTime, _rotation.y * Time.deltaTime, _rotation.z * Time.deltaTime, Space.Self);

            if (!isRotated)
            {
                isRotated = true;
            }
        }
    }


    // Выбор Интерактивного объекта.
    public void OpenInteractable()
    {
        if (!isChoosed)
        {
            isChoosed = true;
            SetMaterialForObjects(_choosedMaterial);
        }

        if (isAnimated)
        {
            AppManager.Instanse.AnimateCurrentInteractable();

            AppManager.Instanse.DisableAllCursors();

            EnableDisplayCursor();

            if (isMultipleParts)
            {
                AppManager.Instanse.ChooseAnimateCurrentInteractable(AnimatorNumberOfObject);
            }
        }

        _instantiatedCursor.gameObject.SetActive(false);
    }

    // Закрытие интерактивного объекта.
    public void CloseInteractable()
    {
        if (isChoosed)
        {
            isChoosed = false;
            SetMaterialForObjects(_defaultMaterial);
            if (isRotated)
            {
                _currentRotation = _rootTransform.localRotation;
                StartCoroutine(RestartRotation(_currentRotation, _startRotation, 0.5f));
            }
            AppManager.Instanse.StopAnimateCurrentInteractable();
        }

        if (_isCursorSetUp)
        {
            _instantiatedCursor.gameObject.SetActive(true);
        }

    }

    public void ResetInteractable()
    {
        _rootTransform.localScale = Vector3.one;
        _rootTransform.localRotation = _startRotation;
        isChoosed = false;
        SetMaterialForObjects(_defaultMaterial);
    }

    public string GetDescription()
    {
        return ObjectDescription;
    }

    // Изменение материалов объектов.
    public void SetMaterialForObjects(Material materialToChangeTo)
    {
        for (int i = 0; i < ObjectsToChangeMaterial.Length; i++)
        {
            ObjectsToChangeMaterial[i].material = materialToChangeTo;

            if (ObjectsToChangeMaterial[i].materials.Length > 1)
            {

                Material[] allMaterials = ObjectsToChangeMaterial[i].materials;

                for (int x = 0; x < allMaterials.Length; x++)
                {
                    allMaterials[x] = materialToChangeTo;
                }

                ObjectsToChangeMaterial[i].materials = allMaterials;
            }
        }
    }

    // Настройка материалов для объектов.
    public void SetUpInteractableMaterials(Material defaultMaterial, Material choosedMaterial)
    {
        _defaultMaterial = defaultMaterial;
        _choosedMaterial = choosedMaterial;
    }

    // Настройка компонента.
    public void SetUpTransform()
    {
        _rootTransform = transform.GetChild(0).GetComponent<Transform>();
        _startRotation = _rootTransform.localRotation;
    }

    // Включение/Отключение синего курсора.
    public void EnableDisplayCursor()
    {

        _instantiatedCursor.gameObject.SetActive(true);

    }

    public void DisableDisplayCursor()
    {
        _instantiatedCursor.gameObject.SetActive(false);
    }

    // Красивое вращение к начальному состоянию.
    public IEnumerator RestartRotation(Quaternion currentRotation, Quaternion startRotation, float duration)
    {
        Quaternion curRot = currentRotation;
        Quaternion startRot = startRotation;

        float timer = 0;

        while (timer <= duration)
        {
            float percentage = timer / duration;
            _rootTransform.localRotation = Quaternion.Lerp(curRot, startRot, percentage);
            timer += Time.deltaTime;
            yield return null;
        }

        if (timer >= duration)
        {
            _rootTransform.localRotation = startRot;
        }

        isRotated = false;
    }



}
