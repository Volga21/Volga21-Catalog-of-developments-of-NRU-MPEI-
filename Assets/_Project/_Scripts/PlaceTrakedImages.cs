using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceTrakedImages : MonoBehaviour
{
    // Reference to AR tracked image manager component
    private ARTrackedImageManager _trackedImagesManager;

    public Model[] ArPrefabs;

    private readonly Dictionary<string, Model> _instantiatedPrefabs = new Dictionary<string, Model>();

    private void Update()
    {
        if (AppManager.Instanse.Debug)
        {
            if (Input.GetMouseButtonDown(1))
            {
                AppManager.Instanse.ImageisSet();
            }
        }

    }


    void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }


    private void OnEnable()
    {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }


    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            var imageName = trackedImage.referenceImage.name;
            foreach (var curPrefab in ArPrefabs)
            {
                if (string.Compare(curPrefab.name, imageName, StringComparison.OrdinalIgnoreCase) == 0 && !_instantiatedPrefabs.ContainsKey(imageName))
                {
                    Vector3 imagePos = trackedImage.transform.position;
                    Quaternion imageRot = trackedImage.transform.rotation;

                    AppManager.Instanse.ImageisSet();
                    AppManager.Instanse.SetupCurrentModel(imagePos, imageRot);
                    AppManager.Instanse.PlaceCurrentModel(trackedImage.transform.position);

                    AppManager.Instanse.CurrentAppState = AppState.ViewDescription;
                    AppManager.Instanse.ChangeCurrentState();
                    // // Создание модели
                    // var newPrefab = Instantiate(curPrefab);
                    // // Присвоение модели позиции и вращения метки.
                    // newPrefab.transform.position = imagePos;
                    // // Сокрытие модели и оповещение AppManager.
                    // _instantiatedPrefabs.Add(imageName, newPrefab);
                    // _instantiatedPrefabs[trackedImage.referenceImage.name].gameObject.SetActive(true);
                    // AppManager.Instanse.SetupInstantiatedObject(newPrefab);
                }
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            AppManager.Instanse.PlaceCurrentModel(trackedImage.transform.position);
        }
    }
}
